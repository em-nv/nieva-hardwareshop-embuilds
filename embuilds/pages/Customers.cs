using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

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

        private void btnExport_Click(object sender, EventArgs e)
        {
            string templatePath = Application.StartupPath + @"\reportTemplate\CustomerList.xlsx";
            DateTime now = DateTime.Now;
            string mydate = now.ToString("yyyy-mm-dd-hh-mm-ss");
            string newFilePath = Application.StartupPath + @"\generatedreports\CustomerReport-" + mydate + ".xlsx";

            // Create directory if it doesn't exist
            if (!Directory.Exists(Application.StartupPath + @"\generatedreports"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\generatedreports");
            }

            ExportDataGridViewToExcelTemplate(dataGridCustomers, templatePath, newFilePath);
        }

        private void ExportDataGridViewToExcelTemplate(DataGridView dgv, string templatePath, string newFilePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Verify template exists
                if (!File.Exists(templatePath))
                {
                    MessageBox.Show("Template file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Start Excel
                excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;

                // Open the template
                workbook = excelApp.Workbooks.Open(templatePath);
                worksheet = (Excel.Worksheet)workbook.Worksheets[1];

                int startRow = 9; // Starting row in Excel
                int totalColumns = dgv.Columns.Count - 1; // Exclude last column

                // Export data (excluding last column)
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int col = 0; col < totalColumns; col++)
                        {
                            worksheet.Cells[startRow, col + 1] = row.Cells[col].Value?.ToString() ?? "";
                        }
                        startRow++;
                    }
                }

                // Save and show
                workbook.SaveAs(newFilePath);
                excelApp.Visible = true;
                workbook.PrintPreview();

                MessageBox.Show("Report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Customers customers = new Customers();
                customers.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //finally
            //{
            //    // Cleanup COM objects
            //    if (workbook != null)
            //    {
            //        workbook.Close(false);
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            //    }
            //    if (excelApp != null)
            //    {
            //        excelApp.Quit();
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            //    }
            //    if (worksheet != null)
            //    {
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            //    }
            //}
        }
    }
}
