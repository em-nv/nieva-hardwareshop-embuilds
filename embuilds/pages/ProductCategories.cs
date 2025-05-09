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
    public partial class ProductCategories : Form
    {
        public ProductCategories()
        {
            InitializeComponent();
        }

        private void frmProductCategories_Load(object sender, EventArgs e)
        {
            var conn_db = new conn_DB();
            dataGridProductCategories.DataSource = conn_db.GetAllProductCategories();
            dataGridProductCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add Edit button column
            if (!dataGridProductCategories.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.Name = "Edit";
                editButton.HeaderText = "Action";
                editButton.Text = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dataGridProductCategories.Columns.Add(editButton);
            }

            // Handle button click event
            dataGridProductCategories.CellClick += dataGridProductCategories_CellClick;
        }

        private void dataGridProductCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid and the column is "Edit"
            if (e.RowIndex >= 0 && dataGridProductCategories.Columns[e.ColumnIndex].Name == "Edit")
            {
                try
                {
                    // Retrieve the selected row
                    var selectedRow = dataGridProductCategories.Rows[e.RowIndex];

                    // Safely extract ProductID (make sure column name matches your data)
                    if (selectedRow.Cells["id"].Value != null) // Ensure "id" matches your column name
                    {
                        int productCategoryId = Convert.ToInt32(selectedRow.Cells["id"].Value);

                        ProductCategoryEdit productCategoryEdit = new ProductCategoryEdit();
                        productCategoryEdit.ProductCategoryId = productCategoryId;
                        this.Hide();

                        productCategoryEdit.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Category ID is missing for the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while trying to open the category editor:\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void btnAddNewCategory_Click(object sender, EventArgs e)
        {
            ProductCategoryAdd productCategoryAdd = new ProductCategoryAdd();
            productCategoryAdd.Show();
            this.Hide();
        }
    }
}
