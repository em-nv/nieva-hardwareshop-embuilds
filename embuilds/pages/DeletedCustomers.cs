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
    public partial class DeletedCustomers : Form
    {
        public DeletedCustomers()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Hide();
        }

        private void DeletedCustomers_Load(object sender, EventArgs e)
        {
            LoadDeletedCustomers();
        }

        private void LoadDeletedCustomers()
        {
            var conn_db = new conn_DB();
            dataGridDeletedCustomers.DataSource = conn_db.GetAllDeletedCustomers();
            dataGridDeletedCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add Restore button column if it doesn't exist
            if (!dataGridDeletedCustomers.Columns.Contains("Restore"))
            {
                DataGridViewButtonColumn restoreButton = new DataGridViewButtonColumn();
                restoreButton.Name = "Restore";
                restoreButton.HeaderText = "Action";
                restoreButton.Text = "Restore";
                restoreButton.UseColumnTextForButtonValue = true;
                dataGridDeletedCustomers.Columns.Add(restoreButton);
            }

            // Handle button click event
            dataGridDeletedCustomers.CellClick += DataGridDeletedCustomers_CellClick;
        }

        private void DataGridDeletedCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridDeletedCustomers.Rows.Count)
                return;

            if (dataGridDeletedCustomers.Columns[e.ColumnIndex].Name == "Restore")
            {
                try
                {
                    var selectedRow = dataGridDeletedCustomers.Rows[e.RowIndex];
                    int deletedRecordId = Convert.ToInt32(selectedRow.Cells["id"].Value);

                    string firstName = selectedRow.Cells["First Name"].Value?.ToString() ?? "";
                    string lastName = selectedRow.Cells["Last Name"].Value?.ToString() ?? "";
                    string customerName = $"{firstName} {lastName}".Trim();

                    var confirmResult = MessageBox.Show($"Restore customer: {customerName}?",
                                                     "Confirm Restore",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

                    if (confirmResult == DialogResult.Yes)
                    {
                        var conn_db = new conn_DB();
                        bool success = conn_db.RestoreCustomer(deletedRecordId);

                        if (success)
                        {
                            MessageBox.Show("Customer restored successfully!", "Success",
                                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDeletedCustomers();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error restoring customer: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
