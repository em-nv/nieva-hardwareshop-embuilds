using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

            // Add Edit button column if it doesn't exist
            if (!dataGridCustomers.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.Name = "Edit";
                editButton.HeaderText = "Action";
                editButton.Text = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dataGridCustomers.Columns.Add(editButton);
            }

            // Add Delete button column if it doesn't exist
            if (!dataGridCustomers.Columns.Contains("Delete"))
            {
                DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                deleteButton.Name = "Delete";
                deleteButton.HeaderText = "Action";
                deleteButton.Text = "Delete";
                deleteButton.UseColumnTextForButtonValue = true;
                dataGridCustomers.Columns.Add(deleteButton);
            }

            // Handle button click event
            dataGridCustomers.CellClick += dataGridCustomers_CellClick;
        }

        private void dataGridCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid (not header row and within range)
            if (e.RowIndex < 0 || e.RowIndex >= dataGridCustomers.Rows.Count)
                return;

            // Retrieve the selected row
            DataGridViewRow selectedRow = dataGridCustomers.Rows[e.RowIndex];

            try
            {
                // Handle Edit button click
                if (dataGridCustomers.Columns[e.ColumnIndex].Name == "Edit")
                {
                    // Safely extract CustomerID
                    if (selectedRow.Cells["id"].Value != null)
                    {
                        int customerId = Convert.ToInt32(selectedRow.Cells["id"].Value);

                        CustomerEdit customerEdit = new CustomerEdit();
                        customerEdit.CustomerId = customerId;
                        this.Hide();
                        customerEdit.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Customer ID is missing for the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Handle Delete button click
                else if (dataGridCustomers.Columns[e.ColumnIndex].Name == "Delete")
                {
                    // Safely extract CustomerID
                    if (selectedRow.Cells["id"].Value == null)
                    {
                        MessageBox.Show("Customer ID is missing for the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int customerId = Convert.ToInt32(selectedRow.Cells["id"].Value);
                    string customerName = GetCustomerName(selectedRow);

                    // Show confirmation dialog
                    var confirmResult = MessageBox.Show($"Are you sure you want to delete customer: {customerName}?",
                                                    "Confirm Delete",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Warning);

                    if (confirmResult == DialogResult.Yes)
                    {
                        // Perform deletion
                        var conn_db = new conn_DB();
                        bool success = conn_db.DeleteCustomer(customerId);

                        if (success)
                        {
                            MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Refresh the grid
                            dataGridCustomers.DataSource = conn_db.GetAllCustomers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while processing your request:\n" + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += "\n\nDetails: " + ex.InnerException.Message;
                }
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to safely get customer name
        private string GetCustomerName(DataGridViewRow row)
        {
            try
            {
                // Try different possible column name combinations
                if (ColumnExists(row, "full_name"))
                    return row.Cells["full_name"].Value.ToString();

                if (ColumnExists(row, "name"))
                    return row.Cells["name"].Value.ToString();

                if (ColumnExists(row, "first_name") && ColumnExists(row, "last_name"))
                    return $"{row.Cells["first_name"].Value} {row.Cells["last_name"].Value}";

                if (ColumnExists(row, "customer_name"))
                    return row.Cells["customer_name"].Value.ToString();

                // If no name columns found, return generic text with ID
                return $"Customer ID: {row.Cells["id"].Value}";
            }
            catch
            {
                return "this customer";
            }
        }

        // Improved column exists check
        private bool ColumnExists(DataGridViewRow row, string columnName)
        {
            return dataGridCustomers.Columns.Contains(columnName) &&
                   row.Cells[columnName].Value != null &&
                   !string.IsNullOrWhiteSpace(row.Cells[columnName].Value.ToString());
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
            string templatePath = Path.Combine(Application.StartupPath, @"reportTemplate\CustomerList.xlsx");

            if (!File.Exists(templatePath))
            {
                MessageBox.Show("Template file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files|*.xlsx";
                saveDialog.Title = "Save Customer Report";
                saveDialog.FileName = $"CustomersReport-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
                saveDialog.DefaultExt = "xlsx";
                saveDialog.AddExtension = true;

                // Set default directory to user's Documents
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportDataGridViewToExcelTemplate(dataGridCustomers, templatePath, saveDialog.FileName);

                        // Open the saved file directly in Excel
                        System.Diagnostics.Process.Start(saveDialog.FileName);

                        MessageBox.Show($"Report saved and opened successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Export failed: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportDataGridViewToExcelTemplate(DataGridView dgv, string templatePath, string savePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = false; // Keep Excel hidden during export
                excelApp.DisplayAlerts = false;

                workbook = excelApp.Workbooks.Open(templatePath);
                worksheet = (Excel.Worksheet)workbook.Worksheets[1];

                int startRow = 9;
                int totalColumns = dgv.Columns.Count - 1;

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

                // Save the file and close Excel
                workbook.SaveAs(savePath);
            }
            finally
            {
                // Cleanup COM objects
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null)
                {
                    workbook.Close(false);
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        private void btnTrash_Click(object sender, EventArgs e)
        {
            DeletedCustomers deletedCustomers = new DeletedCustomers();
            deletedCustomers.Show();
            this.Hide();
        }
    }
}
