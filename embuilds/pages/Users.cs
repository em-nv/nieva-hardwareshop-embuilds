using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Bcpg;
using Excel = Microsoft.Office.Interop.Excel;

namespace embuilds.pages
{
    public partial class Users : Form
    {
        private DataTable originalUsersData;
        public Users()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            RefreshDataGrid();
            dataGridUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            textBoxSearchUser.TextChanged += textBoxSearchUser_TextChanged;
            dataGridUsers.CellClick += dataGridUsers_CellClick;
        }

        private void textBoxSearchUser_TextChanged(object sender, EventArgs e)
        {
            // Always get fresh data from database when search is cleared
            if (string.IsNullOrEmpty(textBoxSearchUser.Text.Trim()))
            {
                RefreshDataGrid();
                return;
            }

            // Proceed with filtering if we have search text
            if (originalUsersData != null)
            {
                string searchText = textBoxSearchUser.Text.Trim();

                // Create a filtered view
                DataView dv = originalUsersData.DefaultView;

                // Build a proper filter expression
                List<string> filterParts = new List<string>();

                foreach (DataColumn column in originalUsersData.Columns)
                {
                    // Only include string columns that exist in the grid (excluding buttons)
                    if (column.DataType == typeof(string) &&
                        dataGridUsers.Columns.Contains(column.ColumnName) &&
                        column.ColumnName != "Edit" &&
                        column.ColumnName != "Delete")
                    {
                        // Properly escape single quotes in the search text
                        string escapedSearchText = searchText.Replace("'", "''");
                        filterParts.Add($"[{column.ColumnName}] LIKE '%{escapedSearchText}%'");
                    }
                }

                if (filterParts.Count > 0)
                {
                    // Combine with OR to search across all columns
                    dv.RowFilter = string.Join(" OR ", filterParts);
                    dataGridUsers.DataSource = dv.ToTable();
                }
                else
                {
                    // No searchable columns found, show all data
                    dataGridUsers.DataSource = originalUsersData;
                }
            }
        }

        private void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridUsers.Rows[e.RowIndex];

                // Ensure "id" column exists and has value
                if (selectedRow.Cells["id"].Value == null)
                {
                    MessageBox.Show("User ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int userId = Convert.ToInt32(selectedRow.Cells["id"].Value);

                // Handle Edit
                if (dataGridUsers.Columns[e.ColumnIndex].Name == "Edit")
                {
                    UserEdit userEdit = new UserEdit();
                    userEdit.UserId = userId;
                    this.Hide();
                    userEdit.ShowDialog();
                }

                // Handle Delete
                else if (dataGridUsers.Columns[e.ColumnIndex].Name == "Delete")
                {
                    var confirmResult = MessageBox.Show("Are you sure you want to delete this user?",
                                                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirmResult == DialogResult.Yes)
                    {
                        try
                        {
                            conn_DB db = new conn_DB();
                            using (MySql.Data.MySqlClient.MySqlConnection conn = db.GetConnection())
                            {
                                conn.Open();

                                string query = "DELETE FROM users WHERE id = @id";
                                using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@id", userId);
                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        // Refresh the grid with fresh data from DB
                                        RefreshDataGrid();
                                    }
                                    else
                                    {
                                        MessageBox.Show("User could not be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred while deleting the user:\n" + ex.Message,
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            UserAdd userAdd = new UserAdd();
            userAdd.Show();
            this.Hide();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                string templatePath = Path.Combine(Application.StartupPath, @"reportTemplate\UserList.xlsx");

                // Verify template exists
                if (!File.Exists(templatePath))
                {
                    MessageBox.Show("Template file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files|*.xlsx";
                    saveDialog.Title = "Save User Report";
                    saveDialog.FileName = $"UserReport-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.xlsx";
                    saveDialog.DefaultExt = "xlsx";
                    saveDialog.AddExtension = true;
                    saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        excelApp = new Excel.Application();
                        excelApp.Visible = false;
                        excelApp.DisplayAlerts = false;

                        // Open template
                        workbook = excelApp.Workbooks.Open(templatePath, ReadOnly: false);
                        worksheet = (Excel.Worksheet)workbook.Worksheets[1];

                        // Export data
                        var columnsToExport = dataGridUsers.Columns
                            .Cast<DataGridViewColumn>()
                            .Where(c => c.Name != "Edit" && c.Name != "Delete")
                            .ToList();

                        // Write headers
                        for (int i = 0; i < columnsToExport.Count; i++)
                        {
                            worksheet.Cells[1, i + 1] = columnsToExport[i].HeaderText;
                        }

                        // Write data
                        int rowIndex = 9;
                        foreach (DataGridViewRow row in dataGridUsers.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                for (int col = 0; col < columnsToExport.Count; col++)
                                {
                                    worksheet.Cells[rowIndex, col + 1] = row.Cells[columnsToExport[col].Index].Value?.ToString() ?? "";
                                }
                                rowIndex++;
                            }
                        }

                        // Save and close
                        workbook.SaveAs(saveDialog.FileName);
                        workbook.Close(false);
                        excelApp.Quit();

                        // Release COM objects in reverse order
                        Marshal.ReleaseComObject(worksheet);
                        Marshal.ReleaseComObject(workbook);
                        Marshal.ReleaseComObject(excelApp);
                        worksheet = null;
                        workbook = null;
                        excelApp = null;

                        // Open the file
                        Process.Start(new ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });

                        MessageBox.Show("Report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Final cleanup in case of exceptions
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
                int totalColumns = dgv.Columns.Count - 2; // Exclude last column

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

                Users users = new Users();
                users.Show();
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

        private void RefreshDataGrid()
        {
            var conn_db = new conn_DB();
            originalUsersData = conn_db.GetAllUsers();

            // If there's search text, reapply the filter
            if (!string.IsNullOrEmpty(textBoxSearchUser.Text.Trim()))
            {
                textBoxSearchUser_TextChanged(null, EventArgs.Empty);
            }
            else
            {
                // No search text, show all data
                dataGridUsers.DataSource = originalUsersData;
            }

            // Ensure buttons are always present
            EnsureButtonColumns();
        }

        private void EnsureButtonColumns()
        {
            // Add Edit button column if not exists
            if (!dataGridUsers.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.Name = "Edit";
                editButton.HeaderText = "Action";
                editButton.Text = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dataGridUsers.Columns.Add(editButton);
            }

            // Add Delete button column if not exists
            if (!dataGridUsers.Columns.Contains("Delete"))
            {
                DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                deleteButton.Name = "Delete";
                deleteButton.HeaderText = "Action";
                deleteButton.Text = "Delete";
                deleteButton.UseColumnTextForButtonValue = true;
                dataGridUsers.Columns.Add(deleteButton);
            }
        }
    }
}
