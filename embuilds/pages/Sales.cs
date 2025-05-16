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
        private DataTable originalSalesData;
        public Sales()
        {
            InitializeComponent();
            comboBoxFilter.DropDownStyle = ComboBoxStyle.DropDownList;
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
            // Initialize filter combo box (if not done in designer)
            if (comboBoxFilter.Items.Count == 0)
            {
                comboBoxFilter.Items.AddRange(new string[] { "All", "Today", "Yesterday", "This Month", "This Year" });
                comboBoxFilter.SelectedIndex = 0;
            }

            // Load data
            RefreshDataGrid();
            dataGridSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set up event handlers
            comboBoxFilter.SelectedIndexChanged += comboBoxFilter_SelectedIndexChanged;
            textBoxSearchSales.TextChanged += textBoxSearchSales_TextChanged;
        }

        private void RefreshDataGrid()
        {
            try
            {
                var conn_db = new conn_DB();
                originalSalesData = conn_db.GetAllSales();

                // Ensure Date column exists and is proper type
                if (originalSalesData.Columns["Date"] == null)
                {
                    MessageBox.Show("Date column not found in database results", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            if (originalSalesData == null || originalSalesData.Columns["Date"] == null)
            {
                MessageBox.Show("Date column not found in data", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataView filteredView = originalSalesData.DefaultView;
            string dateFilter = GetDateFilter();
            string searchText = textBoxSearchSales.Text.Trim();

            // Build complete filter
            string completeFilter = dateFilter;

            if (!string.IsNullOrEmpty(searchText))
            {
                string searchFilter = GetSearchFilter(searchText);
                completeFilter = string.IsNullOrEmpty(completeFilter)
                    ? searchFilter
                    : $"{completeFilter} AND ({searchFilter})";
            }

            filteredView.RowFilter = completeFilter;
            dataGridSales.DataSource = filteredView.ToTable();
        }

        private string GetDateFilter()
        {
            DateTime now = DateTime.Now;

            switch (comboBoxFilter.SelectedItem.ToString())
            {
                case "Today":
                    return $"[Date] = #{now.ToString("MM/dd/yyyy")}#";  // Using # for date format
                case "Yesterday":
                    return $"[Date] = #{now.AddDays(-1).ToString("MM/dd/yyyy")}#";
                case "This Month":
                    return $"[Date] >= #{new DateTime(now.Year, now.Month, 1).ToString("MM/dd/yyyy")}# " +
                           $"AND [Date] <= #{now.ToString("MM/dd/yyyy")}#";
                case "This Year":
                    return $"[Date] >= #{new DateTime(now.Year, 1, 1).ToString("MM/dd/yyyy")}# " +
                           $"AND [Date] <= #{now.ToString("MM/dd/yyyy")}#";
                default:
                    return string.Empty;
            }
        }

        private string GetSearchFilter(string searchText)
        {
            List<string> filterParts = new List<string>();
            string escapedSearchText = searchText.Replace("'", "''");

            foreach (DataColumn column in originalSalesData.Columns)
            {
                // Skip Date column for text search (we handle it separately)
                if (column.ColumnName == "Date") continue;

                if (column.DataType == typeof(string))
                {
                    filterParts.Add($"[{column.ColumnName}] LIKE '%{escapedSearchText}%'");
                }
                else if (column.DataType == typeof(decimal) || column.DataType == typeof(int))
                {
                    if (decimal.TryParse(searchText, out decimal number))
                    {
                        filterParts.Add($"[{column.ColumnName}] = {number}");
                    }
                }
            }

            return filterParts.Count > 0 ? string.Join(" OR ", filterParts) : "1=1"; // Return true condition if no filters
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void textBoxSearchSales_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
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

            try
            {
                if (!File.Exists(templatePath))
                {
                    MessageBox.Show("Template file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                excelApp = new Excel.Application();
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;

                Excel.Workbook workbook = excelApp.Workbooks.Open(templatePath);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];

                int startRow = 9;
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
                }

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int col = 0; col < dgv.Columns.Count; col++)
                        {
                            worksheet.Cells[startRow, col + 1] = row.Cells[col].Value?.ToString() ?? "";
                        }
                        startRow++;
                    }
                }

                workbook.SaveAs(newFilePath);
                excelApp.Visible = true;

                MessageBox.Show($"Report exported successfully to:\n{newFilePath}", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                excelApp?.Quit();
            }
        }
    }
}
