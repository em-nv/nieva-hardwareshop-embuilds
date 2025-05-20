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

        private void btnExport_Click(object sender, EventArgs e)
        {
            string templatePath = Path.Combine(Application.StartupPath, @"reportTemplate\SupplierList.xlsx");

            if (!File.Exists(templatePath))
            {
                MessageBox.Show("Template file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files|*.xlsx";
                saveDialog.Title = "Save Suppliers Report";
                saveDialog.FileName = $"SupplierReport-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
                saveDialog.DefaultExt = "xlsx";
                saveDialog.AddExtension = true;

                // Set default directory to user's Documents
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportDataGridViewToExcelTemplate(dataGridSuppliers, templatePath, saveDialog.FileName);

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
    }
}
