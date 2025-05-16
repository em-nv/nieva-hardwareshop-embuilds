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
    public partial class Dashboard : Form
    {
        private int userId;

        public Dashboard(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        public Dashboard()
        {
            InitializeComponent();
        }

        //private void Products_Click(object sender, EventArgs e)
        //{
        //    Products products = new Products();
        //    products.Show();
        //    this.Hide();
        //}

        private void btnInventory_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory();
            inventory.Show();
            this.Hide();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            products.Show();
            this.Hide();
        }

        private void btnPOS_click(object sender, EventArgs e)
        {
            POS pos = new POS();
            pos.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private Timer dateTimeTimer;
        private void Dashboard_Load(object sender, EventArgs e)
        {
            dateTimeTimer = new Timer();
            dateTimeTimer.Interval = 1000; // 1 second
            dateTimeTimer.Tick += UpdateDateTime;
            dateTimeTimer.Start();

            // Initialize combo box with filter options and make it non-editable
            comboBoxFilter.DropDownStyle = ComboBoxStyle.DropDownList; // Makes it non-editable
            comboBoxFilter.Items.AddRange(new string[] { "Today", "Yesterday", "This Month", "This Year" });
            comboBoxFilter.SelectedIndex = 0; // Default to "Today"

            // Add event handler for when selection changes
            comboBoxFilter.SelectedIndexChanged += FilterSalesData;

            // Load initial data
            FilterSalesData(null, null);
        }

        private void FilterSalesData(object sender, EventArgs e)
        {
            string selectedFilter = comboBoxFilter.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedFilter))
                return;

            DateTime startDate, endDate;

            // Set date range based on selected filter
            switch (selectedFilter)
            {
                case "Today":
                    startDate = DateTime.Today;
                    endDate = DateTime.Today.AddDays(1).AddSeconds(-1);
                    break;
                case "Yesterday":
                    startDate = DateTime.Today.AddDays(-1);
                    endDate = DateTime.Today.AddSeconds(-1);
                    break;
                case "This Month":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    break;
                case "This Year":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                    break;
                default:
                    startDate = DateTime.Today;
                    endDate = DateTime.Today.AddDays(1).AddSeconds(-1);
                    break;
            }

            // Get sales data
            decimal totalSales = 0;
            int productsSold = 0;

            using (var conn = new MySqlConnection(new conn_DB().connectionString))
            {
                conn.Open();

                // Query for total sales
                string salesQuery = @"SELECT SUM(total_price) FROM sales 
                            WHERE created_at BETWEEN @startDate AND @endDate";
                MySqlCommand salesCmd = new MySqlCommand(salesQuery, conn);
                salesCmd.Parameters.AddWithValue("@startDate", startDate);
                salesCmd.Parameters.AddWithValue("@endDate", endDate);

                object salesResult = salesCmd.ExecuteScalar();
                if (salesResult != DBNull.Value)
                {
                    totalSales = Convert.ToDecimal(salesResult);
                }

                // Query for products sold
                string productsQuery = @"SELECT SUM(quantity) FROM sales 
                               WHERE created_at BETWEEN @startDate AND @endDate";
                MySqlCommand productsCmd = new MySqlCommand(productsQuery, conn);
                productsCmd.Parameters.AddWithValue("@startDate", startDate);
                productsCmd.Parameters.AddWithValue("@endDate", endDate);

                object productsResult = productsCmd.ExecuteScalar();
                if (productsResult != DBNull.Value)
                {
                    productsSold = Convert.ToInt32(productsResult);
                }

                conn.Close();
            }

            // Update labels with peso sign formatting
            labelSales.Text = $"₱{totalSales:0.00}"; // Format with peso sign and 2 decimal places
            labelProductsSold.Text = productsSold.ToString();
        }


        private void UpdateDateTime(object sender, EventArgs e)
        {
            dateAndTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void buttonSuppliers_Click(object sender, EventArgs e)
        {
            Suppliers suppliers = new Suppliers();
            suppliers.Show();
            this.Hide();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            Sales sales = new Sales();
            sales.Show();
            this.Hide();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Hide();
        }

        private void dashboardLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Display confirmation dialog only when the user tries to close the form manually
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var answer = MessageBox.Show("Do you want to close the application?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Check the user's response
                if (answer == DialogResult.Yes)
                {
                    // User chose Yes, proceed with closing the application
                    Application.Exit();
                }
                else
                {
                    // User chose No, cancel the close action
                    e.Cancel = true;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Transactions transactions = new Transactions();
            transactions.Show();
            this.Hide();
        }

        private void btnProductCategories_Click(object sender, EventArgs e)
        {
            ProductCategories productCategories = new ProductCategories();
            productCategories.Show();
            this.Hide();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Show();
            this.Hide();
        }

        private void btnManageAccount_Click(object sender, EventArgs e)
        {
            MyAccount myAccount = new MyAccount(userId);
            myAccount.Show();
            this.Hide();
        }
    }
}
