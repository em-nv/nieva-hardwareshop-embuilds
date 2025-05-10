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
    public partial class Sales : Form
    {
        public Sales()
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

        private void frmSales_Load(object sender, EventArgs e)
        {
            var conn_db = new conn_DB();
            dataGridSales.DataSource = conn_db.GetAllSales();
            dataGridSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnAddSales_Click(object sender, EventArgs e)
        {
            SalesAdd1 salesAdd1 = new SalesAdd1();
            salesAdd1.Show();
            this.Hide();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string templatePath = Application.StartupPath + @"\reportTemplate\SalesList.xlsx";
            DateTime now = DateTime.Now;
            string mydate = now.ToString("yyyy-mm-dd-hh-mm-ss");
            string newFilePath = Application.StartupPath + @"\generatedreports\SalesReport-" + mydate + ".xlsx";

            // Create directory if it doesn't exist
            if (!Directory.Exists(Application.StartupPath + @"\generatedreports"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\generatedreports");
            }

            ExportDataGridViewToExcelTemplate(dataGridSales, templatePath, newFilePath);
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

                int startRow = 2; // Starting row in Excel
                int totalColumns = dgv.Columns.Count;

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
                Sales sales = new Sales();
                sales.Show();
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
