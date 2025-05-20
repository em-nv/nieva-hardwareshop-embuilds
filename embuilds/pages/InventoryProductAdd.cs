using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace embuilds.pages
{
    public partial class InventoryProductAdd : Form
    {
        conn_DB db = new conn_DB();
        public InventoryProductAdd()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory();
            inventory.Show();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Get values from form controls
            string name = textBoxProductName.Text.Trim();
            string description = textBoxDescription.Text.Trim(); // Description is optional
            int categoryId = Convert.ToInt32(comboBoxCategory.SelectedValue);
            int supplierId = Convert.ToInt32(comboBoxSupplier.SelectedValue);
            decimal price = 0;
            int quantity = 0;

            // Validate required fields (name, category, supplier, price, quantity)
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Product name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (categoryId == 0) // Ensure category is selected
            {
                MessageBox.Show("Category is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (supplierId == 0) // Ensure supplier is selected
            {
                MessageBox.Show("Supplier is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(textBoxPrice.Text.Trim(), out price) || price <= 0)
            {
                MessageBox.Show("Price must be a valid number greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBoxQuantity.Text.Trim(), out quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity must be a valid non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Proceed to save the product if validation passes
            using (MySqlConnection conn = new MySqlConnection("server=localhost; database=hardwareshopdb; uid=root; pwd=emman;"))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert into products
                    string productQuery = @"
                INSERT INTO products (name, description, category_id, supplier_id, price, created_at, updated_at)
                VALUES (@name, @description, @category_id, @supplier_id, @price, NOW(), NOW());
                SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(productQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@category_id", categoryId);
                    cmd.Parameters.AddWithValue("@supplier_id", supplierId);
                    cmd.Parameters.AddWithValue("@price", price);

                    int productId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Update inventory (only stock_available and timestamps)
                    string updateInventory = @"
                UPDATE inventories
                SET stock_available = @quantity,
                    created_at = NOW(),
                    updated_at = NOW()
                WHERE product_id = @product_id";

                    MySqlCommand updateCmd = new MySqlCommand(updateInventory, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@quantity", quantity);
                    updateCmd.Parameters.AddWithValue("@product_id", productId);
                    updateCmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Product saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Redirect to Inventory form
                    Inventory inventory = new Inventory();
                    inventory.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error saving product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InventoryProductAdd_Load(object sender, EventArgs e)
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
