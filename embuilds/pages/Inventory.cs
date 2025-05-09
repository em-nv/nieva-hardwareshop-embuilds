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
    public partial class Inventory : Form
    {
        public Inventory()
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

        private void frmInventory_Load(object sender, EventArgs e)
        {
            var conn_db = new conn_DB();
            dataGridInventory.DataSource = conn_db.GetAllInventory();
            dataGridInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add Edit button column
            if (!dataGridInventory.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.Name = "Edit";
                editButton.HeaderText = "Action";
                editButton.Text = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dataGridInventory.Columns.Add(editButton);
            }

            // Handle button click event
            dataGridInventory.CellClick += dataGridInventory_CellClick;
        }

        private void dataGridInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid and the column is "Edit"
            if (e.RowIndex >= 0 && dataGridInventory.Columns[e.ColumnIndex].Name == "Edit")
            {
                try
                {
                    // Retrieve the selected row
                    var selectedRow = dataGridInventory.Rows[e.RowIndex];

                    // Safely extract ProductID (make sure column name matches your data)
                    if (selectedRow.Cells["id"].Value != null) // Ensure "id" matches your column name
                    {
                        int productId = Convert.ToInt32(selectedRow.Cells["id"].Value);

                        // Use the conn_DB class to create a new instance of the ProductEdit form
                        InventoryProductEdit inventoryProductEdit = new InventoryProductEdit();
                        inventoryProductEdit.ProductId = productId;
                        this.Hide();

                        inventoryProductEdit.ShowDialog(); // Open the ProductEdit form modally
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
            InventoryProductAdd inventoryProductAdd = new InventoryProductAdd();
            inventoryProductAdd.Show();
            this.Hide();
        }
    }
}
