using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace embuilds.pages
{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void frmCustomerList_Load(object sender, EventArgs e)
        {
            var conn_db = new conn_DB();
            dataGridCustomers.DataSource = conn_db.GetAllCustomers();
            dataGridCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add Edit button column
            if (!dataGridCustomers.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.Name = "Edit";
                editButton.HeaderText = "Action";
                editButton.Text = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dataGridCustomers.Columns.Add(editButton);
            }

            // Handle button click event
            dataGridCustomers.CellClick += dataGridCustomers_CellClick;
        }

        private void dataGridCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid and the column is "Edit"
            if (e.RowIndex >= 0 && dataGridCustomers.Columns[e.ColumnIndex].Name == "Edit")
            {
                try
                {
                    // Retrieve the selected row
                    var selectedRow = dataGridCustomers.Rows[e.RowIndex];

                    // Safely extract ProductID (make sure column name matches your data)
                    if (selectedRow.Cells["id"].Value != null) // Ensure "id" matches your column name
                    {
                        int customerId = Convert.ToInt32(selectedRow.Cells["id"].Value);

                        CustomerEdit customerEdit = new CustomerEdit();
                        customerEdit.CustomerId = customerId;
                        this.Hide();

                        //ProductCategoryEdit productCategoryEdit = new ProductCategoryEdit();
                        //productCategoryEdit.ProductCategoryId = productCategoryId;
                        //this.Hide();

                        customerEdit.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Customer ID is missing for the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while trying to open the category editor:\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            CustomerAdd customerAdd = new CustomerAdd();
            customerAdd.Show();
            this.Hide();
        }
    }
}
