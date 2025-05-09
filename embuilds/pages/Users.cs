using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Bcpg;

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
    }
}
