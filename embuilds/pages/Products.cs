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
    public partial class Products : Form
    {
        conn_DB db = new conn_DB();

        public Products()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmProductList_Load(object sender, EventArgs e)
        {
            var conn_db = new conn_DB();
            dataGridProducts.DataSource = conn_db.GetAllProducts();
            dataGridProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add Edit button column
            if (!dataGridProducts.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.Name = "Edit";
                editButton.HeaderText = "Action";
                editButton.Text = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dataGridProducts.Columns.Add(editButton);
            }

            // Handle button click event
            dataGridProducts.CellClick += dataGridProducts_CellClick;
        }

        private void dataGridProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid and the column is "Edit"
            if (e.RowIndex >= 0 && dataGridProducts.Columns[e.ColumnIndex].Name == "Edit")
            {
                try
                {
                    // Retrieve the selected row
                    var selectedRow = dataGridProducts.Rows[e.RowIndex];

                    // Safely extract ProductID (make sure column name matches your data)
                    if (selectedRow.Cells["id"].Value != null) // Ensure "id" matches your column name
                    {
                        int productId = Convert.ToInt32(selectedRow.Cells["id"].Value);

                        // Use the conn_DB class to create a new instance of the ProductEdit form
                        ProductEdit productEdit = new ProductEdit();
                        productEdit.ProductId = productId; // Pass the ProductId to the ProductEdit form
                        this.Hide();

                        productEdit.ShowDialog(); // Open the ProductEdit form modally
                    }
                    else
                    {
                        MessageBox.Show("Product ID is missing for the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while trying to open the product editor:\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        private void btnAddNewProduct_Click(object sender, EventArgs e)
        {
            ProductAdd productAdd = new ProductAdd();
            productAdd.Show();
            this.Hide();
        }
    }
}
