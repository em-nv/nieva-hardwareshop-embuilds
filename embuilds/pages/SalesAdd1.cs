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
    public partial class SalesAdd1 : Form
    {
        conn_DB db = new conn_DB();
        public SalesAdd1()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmSalesAdd1_Load(object sender, EventArgs e)
        {
            conn_DB db = new conn_DB();
            comboBoxCustomerName.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProduct.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    // Load customers
                    string queryCustomers = "SELECT id, first_name, middle_name, last_name FROM customers";
                    MySqlCommand cmdCustomers = new MySqlCommand(queryCustomers, conn);
                    MySqlDataReader readerCustomers = cmdCustomers.ExecuteReader();

                    DataTable customers = new DataTable();
                    customers.Load(readerCustomers);

                    // Add full_name column
                    customers.Columns.Add("Full Name", typeof(string));

                    foreach (DataRow row in customers.Rows)
                    {
                        string middleName = row["middle_name"].ToString();
                        row["Full Name"] = $"{row["first_name"]} {(string.IsNullOrWhiteSpace(middleName) ? "" : middleName + " ")}{row["last_name"]}".Trim();
                    }

                    // Insert default row at the top
                    DataRow defaultRowCustomer = customers.NewRow();
                    defaultRowCustomer["id"] = 0;
                    defaultRowCustomer["first_name"] = "";
                    defaultRowCustomer["middle_name"] = "";
                    defaultRowCustomer["last_name"] = "";
                    defaultRowCustomer["Full Name"] = "- Select Customer -";
                    customers.Rows.InsertAt(defaultRowCustomer, 0);

                    // Bind data to the ComboBox
                    comboBoxCustomerName.DataSource = customers;
                    comboBoxCustomerName.DisplayMember = "Full Name";
                    comboBoxCustomerName.ValueMember = "id";

                    // Load products with available stock
                    string queryProducts = @"
                SELECT p.id, p.name, p.price 
                FROM products p
                INNER JOIN inventories i ON p.id = i.product_id
                WHERE i.stock_status != 'Out of Stock' AND i.stock_available > 0";

                    MySqlCommand cmdProducts = new MySqlCommand(queryProducts, conn);
                    MySqlDataReader readerProducts = cmdProducts.ExecuteReader();

                    DataTable products = new DataTable();
                    products.Load(readerProducts);

                    // Insert default row at the top
                    DataRow defaultRowProduct = products.NewRow();
                    defaultRowProduct["id"] = 0;
                    defaultRowProduct["name"] = "- Select Product -";
                    defaultRowProduct["price"] = 0;
                    products.Rows.InsertAt(defaultRowProduct, 0);

                    // Bind data to the ComboBox
                    comboBoxProduct.DataSource = products;
                    comboBoxProduct.DisplayMember = "name";
                    comboBoxProduct.ValueMember = "id";

                    // Set event handler for product selection change
                    comboBoxProduct.SelectedIndexChanged += ComboBoxProduct_SelectedIndexChanged;

                    // Set event handler for quantity change
                    textBoxQuantity.TextChanged += TextBoxQuantity_TextChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        // Event when a product is selected
        private void ComboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProduct.SelectedIndex > 0) // Only if a real product is selected (not the default)
            {
                DataRowView selectedProduct = (DataRowView)comboBoxProduct.SelectedItem;
                decimal unitPrice = Convert.ToDecimal(selectedProduct["price"]);

                // Store the raw price in Tag property
                textBoxUnitPrice.Tag = unitPrice;

                // Display formatted price with peso sign
                textBoxUnitPrice.Text = unitPrice.ToString("₱#,##0.00");
            }
            else
            {
                textBoxUnitPrice.Tag = 0m;
                textBoxUnitPrice.Clear();
            }

            CalculateTotalPrice();
        }

        // Event when quantity is changed
        private void TextBoxQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }

        // Method to calculate and display the total price
        private void CalculateTotalPrice()
        {
            // Check if a product is selected and quantity is valid
            if (comboBoxProduct.SelectedIndex > 0 &&
                textBoxUnitPrice.Tag != null &&
                decimal.TryParse(textBoxUnitPrice.Tag.ToString(), out decimal unitPrice) &&
                int.TryParse(textBoxQuantity.Text, out int quantity) &&
                quantity > 0)
            {
                decimal totalPrice = unitPrice * quantity;
                textBoxTotalPrice.Text = totalPrice.ToString("₱#,##0.00");
            }
            else
            {
                textBoxTotalPrice.Clear();
            }
        }



        private void btnBack_Click(object sender, EventArgs e)
        {
            Sales sales = new Sales();
            sales.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate product selection
            if (comboBoxProduct.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a product.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxProduct.Focus();
                return;
            }

            // Validate quantity
            if (!int.TryParse(textBoxQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity greater than 0.", "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxQuantity.Focus();
                return;
            }

            // Get selected values
            int productId = Convert.ToInt32(comboBoxProduct.SelectedValue);
            object customerId = comboBoxCustomerName.SelectedIndex > 0
                ? (object)Convert.ToInt32(comboBoxCustomerName.SelectedValue)
                : DBNull.Value;

            try
            {
                using (MySqlConnection conn = new conn_DB().GetConnection())
                {
                    conn.Open();

                    // 1. Check stock availability
                    string checkStockQuery = "SELECT stock_available FROM inventories WHERE product_id = @productId";
                    using (MySqlCommand checkStockCmd = new MySqlCommand(checkStockQuery, conn))
                    {
                        checkStockCmd.Parameters.AddWithValue("@productId", productId);
                        int availableStock = Convert.ToInt32(checkStockCmd.ExecuteScalar());

                        if (availableStock < quantity)
                        {
                            MessageBox.Show($"Insufficient stock. Only {availableStock} items available.",
                                            "Stock Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // 2. Insert sales record (trigger will handle inventory update)
                    string insertSalesQuery = @"
                INSERT INTO sales (customer_id, product_id, quantity, created_at, updated_at)
                VALUES (@customerId, @productId, @quantity, NOW(), NOW())";

                    using (MySqlCommand insertSalesCmd = new MySqlCommand(insertSalesQuery, conn))
                    {
                        insertSalesCmd.Parameters.AddWithValue("@customerId", customerId);
                        insertSalesCmd.Parameters.AddWithValue("@productId", productId);
                        insertSalesCmd.Parameters.AddWithValue("@quantity", quantity);

                        int rowsAffected = insertSalesCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Sale recorded successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Sales sales = new Sales();
                            sales.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Failed to record sale.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error recording sale: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
