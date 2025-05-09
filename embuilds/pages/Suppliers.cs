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
    public partial class Suppliers : Form
    {
        public Suppliers()
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

        private void frmSupplierList_Load(object sender, EventArgs e)
        {
            var conn_db = new conn_DB();
            dataGridSuppliers.DataSource = conn_db.GetAllSuppliers();
            dataGridSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add Edit button column
            if (!dataGridSuppliers.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.Name = "Edit";
                editButton.HeaderText = "Action";
                editButton.Text = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dataGridSuppliers.Columns.Add(editButton);
            }

            // Handle button click event
            dataGridSuppliers.CellClick += dataGridSuppliers_CellClick;
        }

        private void dataGridSuppliers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid and the column is "Edit"
            if (e.RowIndex >= 0 && dataGridSuppliers.Columns[e.ColumnIndex].Name == "Edit")
            {
                try
                {
                    // Retrieve the selected row
                    var selectedRow = dataGridSuppliers.Rows[e.RowIndex];

                    // Safely extract ProductID (make sure column name matches your data)
                    if (selectedRow.Cells["id"].Value != null) // Ensure "id" matches your column name
                    {
                        int supplierId = Convert.ToInt32(selectedRow.Cells["id"].Value);

                        
                        SupplierEdit supplierEdit = new SupplierEdit();
                        supplierEdit.SupplierId = supplierId;
                        this.Hide();

                        supplierEdit.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Supplier ID is missing for the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while trying to open the supplier editor:\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddNewSupplier_Click(object sender, EventArgs e)
        {
            SupplierAdd supplierAdd = new SupplierAdd();
            supplierAdd.Show();
            this.Hide();
        }
    }
}
