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

        private void btnExport_Click(object sender, EventArgs e)
        {
            string templatePath = Path.Combine(Application.StartupPath, @"reportTemplate\InventoryList.xlsx");

            if (!File.Exists(templatePath))
            {
                MessageBox.Show("Template file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files|*.xlsx";
                saveDialog.Title = "Save Inventory Report";
                saveDialog.FileName = $"InventoryReport-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
                saveDialog.DefaultExt = "xlsx";
                saveDialog.AddExtension = true;

                // Set default directory to user's Documents
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportDataGridViewToExcelTemplate(dataGridInventory, templatePath, saveDialog.FileName);

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
