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
    public partial class ResetPassword : Form
    {
        private int userId;
        private conn_DB db;
        public int UserId { get; set; }
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void ResetPassword_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newPassword = textBoxNewPassword.Text.Trim();
            string confirmPassword = textBoxConfirmNewPassword.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill in both password fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Mismatch Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySql.Data.MySqlClient.MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                UPDATE users
                SET password = @password,
                    updated_at = NOW()
                WHERE id = @id";

                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@password", newPassword); // You may want to hash this
                        cmd.Parameters.AddWithValue("@id", UserId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password has been successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Redirect to login or close
                            Login login = new Login();
                            login.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No changes were made. Please try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the password:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Show();
            this.Hide();
        }
    }
}
