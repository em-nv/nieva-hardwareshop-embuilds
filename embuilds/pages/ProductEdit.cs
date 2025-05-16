using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.Cmp;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace embuilds.pages
{
    public partial class ProductEdit : Form
    {
        conn_DB db = new conn_DB();

        public int ProductId { get; set; }

        public ProductEdit()
        {
            InitializeComponent();
        }

        private void ProductEdit_Load(object sender, EventArgs e)
        {
            comboBoxCategory.DropDownStyle = ComboBoxStyle.DropDownList;

            // Load Categories from MySQL
            DataTable categories = db.GetAllProductCategories();
            DataRow categoryRow = categories.NewRow();
            categoryRow["ID"] = 0;
            categoryRow["Category Name"] = "- Select Category -";
            categories.Rows.InsertAt(categoryRow, 0);
            comboBoxCategory.DataSource = categories;
            comboBoxCategory.DisplayMember = "Category Name";
            comboBoxCategory.ValueMember = "ID";

            comboBoxSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            // Load Suppliers from MySQL
            DataTable suppliers = db.GetAllSuppliers();
            DataRow supplierRow = suppliers.NewRow();
            supplierRow["id"] = 0;
            supplierRow["Supplier Name"] = "- Select Supplier -";
            suppliers.Rows.InsertAt(supplierRow, 0);
            comboBoxSupplier.DataSource = suppliers;
            comboBoxSupplier.DisplayMember = "Supplier Name";
            comboBoxSupplier.ValueMember = "id";

            if (ProductId > 0)
            {
                LoadProductDetails();
            }

        }

        private void LoadProductDetails()
        {
            // Check if ProductId is set before proceeding
            if (ProductId <= 0) return;

            try
            {
                // Create an instance of conn_DB and get the connection
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection()) // Use the connection from conn_DB
                {
                    conn.Open(); // Open the connection
                    string query = @"
                SELECT p.name, p.description, p.category_id, p.supplier_id, p.price, i.stock_available
                FROM products p
                JOIN inventories i ON p.id = i.product_id
                WHERE p.id = @id"; // Fetch product details and stock information based on ProductId

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", ProductId); // Use the ProductId passed to the form

                    MySqlDataReader reader = cmd.ExecuteReader(); // Execute the query

                    if (reader.Read()) // Check if data was found
                    {
                        // Load the product data into the form fields
                        textBoxProductName.Text = reader["name"].ToString();
                        textBoxDescription.Text = reader["description"].ToString();
                        comboBoxCategory.SelectedValue = Convert.ToInt32(reader["category_id"]);
                        comboBoxSupplier.SelectedValue = Convert.ToInt32(reader["supplier_id"]);
                        textBoxPrice.Text = reader["price"].ToString();

                        // Load stock available from inventories table
                        textBoxQuantity.Text = reader["stock_available"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Display any errors that occur while loading the product details
                MessageBox.Show("An error occurred while loading product details:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            products.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate required fields (you can expand this validation)
            if (string.IsNullOrWhiteSpace(textBoxProductName.Text) ||
                string.IsNullOrWhiteSpace(textBoxPrice.Text) ||
                comboBoxCategory.SelectedValue == null ||
                comboBoxSupplier.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    // Start a transaction to ensure both updates happen together
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        // Update product details
                        string updateProductQuery = @"
                    UPDATE products
                    SET name = @name,
                        description = @description,
                        category_id = @category_id,
                        supplier_id = @supplier_id,
                        price = @price,
                        updated_at = NOW()
                    WHERE id = @id";

                        MySqlCommand cmdProduct = new MySqlCommand(updateProductQuery, conn, transaction);
                        cmdProduct.Parameters.AddWithValue("@name", textBoxProductName.Text.Trim());
                        cmdProduct.Parameters.AddWithValue("@description", textBoxDescription.Text.Trim());
                        cmdProduct.Parameters.AddWithValue("@category_id", comboBoxCategory.SelectedValue);
                        cmdProduct.Parameters.AddWithValue("@supplier_id", comboBoxSupplier.SelectedValue);
                        cmdProduct.Parameters.AddWithValue("@price", Convert.ToDecimal(textBoxPrice.Text));
                        cmdProduct.Parameters.AddWithValue("@id", ProductId);
                        cmdProduct.ExecuteNonQuery();

                        // Update inventory stock
                        string updateInventoryQuery = @"
                    UPDATE inventories
                    SET stock_available = @stock,
                        updated_at = NOW()
                    WHERE product_id = @product_id";

                        MySqlCommand cmdInventory = new MySqlCommand(updateInventoryQuery, conn, transaction);
                        cmdInventory.Parameters.AddWithValue("@stock", Convert.ToInt32(textBoxQuantity.Text));
                        cmdInventory.Parameters.AddWithValue("@product_id", ProductId);
                        cmdInventory.ExecuteNonQuery();

                        // Commit both updates
                        transaction.Commit();
                    }

                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Products products = new Products();
                    products.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the product:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDecrease_Click(object sender, EventArgs e)
        {
            int quantity = 0;
            int.TryParse(textBoxQuantity.Text, out quantity);
            if (quantity > 0) quantity--; // Prevent going below 0
            textBoxQuantity.Text = quantity.ToString();
        }

        private void buttonIncrease_Click(object sender, EventArgs e)
        {
            int quantity = 0;
            int.TryParse(textBoxQuantity.Text, out quantity);
            quantity++;
            textBoxQuantity.Text = quantity.ToString();
        }
    }
}
