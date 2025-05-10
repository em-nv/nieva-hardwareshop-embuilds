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
using Org.BouncyCastle.Bcpg;
using Excel = Microsoft.Office.Interop.Excel;

namespace embuilds.pages
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            var conn_db = new conn_DB();
            dataGridUsers.DataSource = conn_db.GetAllUsers();
            dataGridUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add Edit button column
            if (!dataGridUsers.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
                editButton.Name = "Edit";
                editButton.HeaderText = "Action";
                editButton.Text = "Edit";
                editButton.UseColumnTextForButtonValue = true;
                dataGridUsers.Columns.Add(editButton);
            }

            // Add Delete button column
            if (!dataGridUsers.Columns.Contains("Delete"))
            {
                DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                deleteButton.Name = "Delete";
                deleteButton.HeaderText = "Action";
                deleteButton.Text = "Delete";
                deleteButton.UseColumnTextForButtonValue = true;
                dataGridUsers.Columns.Add(deleteButton);
            }

            // Handle button click event
            dataGridUsers.CellClick += dataGridUsers_CellClick;
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
                                        // Refresh the grid
                                        dataGridUsers.DataSource = db.GetAllUsers();
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
            string templatePath = Application.StartupPath + @"\reportTemplate\UserList.xlsx";
            DateTime now = DateTime.Now;
            string mydate = now.ToString("yyyy-mm-dd-hh-mm-ss");
            string newFilePath = Application.StartupPath + @"\generatedreports\UserReport-" + mydate + ".xlsx";

            // Create directory if it doesn't exist
            if (!Directory.Exists(Application.StartupPath + @"\generatedreports"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\generatedreports");
            }

            ExportDataGridViewToExcelTemplate(dataGridUsers, templatePath, newFilePath);
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
    }
}
