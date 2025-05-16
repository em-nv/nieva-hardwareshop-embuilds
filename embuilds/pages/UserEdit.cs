using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace embuilds.pages
{
    public partial class UserEdit : Form
    {

        conn_DB db = new conn_DB();
        public int UserId { get; set; }
        public UserEdit()
        {
            InitializeComponent();
        }

        private void UserEdit_Load(object sender, EventArgs e)
        {
            if (UserId > 0)
            {
                UserDetails();
            }
        }

        private void UserDetails()
        {
            if (UserId <= 0) return;

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT id, first_name, middle_name, last_name, username, email, password
                FROM users
                WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", UserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBoxFirstName.Text = reader["first_name"].ToString();
                                textBoxMiddleName.Text = reader["middle_name"].ToString();
                                textBoxLastName.Text = reader["last_name"].ToString();
                                textBoxUserName.Text = reader["username"].ToString();
                                textBoxEmail.Text = reader["email"].ToString();
                                textBoxPassword.Text = reader["password"].ToString();

                            }
                            else
                            {
                                MessageBox.Show("Customer not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading category details:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic input validation (middle name is optional)
            if (string.IsNullOrWhiteSpace(textBoxFirstName.Text) ||
                string.IsNullOrWhiteSpace(textBoxLastName.Text) ||
                string.IsNullOrWhiteSpace(textBoxUserName.Text) ||
                string.IsNullOrWhiteSpace(textBoxEmail.Text) ||
                string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    // Check if the email is already used by another user
                    string emailCheckQuery = "SELECT COUNT(*) FROM users WHERE email = @checkEmail AND id != @currentId";
                    using (MySqlCommand checkEmailCmd = new MySqlCommand(emailCheckQuery, conn))
                    {
                        checkEmailCmd.Parameters.AddWithValue("@checkEmail", textBoxEmail.Text.Trim());
                        checkEmailCmd.Parameters.AddWithValue("@currentId", UserId);

                        long emailCount = (long)checkEmailCmd.ExecuteScalar();

                        if (emailCount > 0)
                        {
                            MessageBox.Show("The email already exists. Please use a different email.", "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Check if the username is already used by another user
                    string usernameCheckQuery = "SELECT COUNT(*) FROM users WHERE username = @checkUsername AND id != @currentId";
                    using (MySqlCommand checkUsernameCmd = new MySqlCommand(usernameCheckQuery, conn))
                    {
                        checkUsernameCmd.Parameters.AddWithValue("@checkUsername", textBoxUserName.Text.Trim());
                        checkUsernameCmd.Parameters.AddWithValue("@currentId", UserId);

                        long usernameCount = (long)checkUsernameCmd.ExecuteScalar();

                        if (usernameCount > 0)
                        {
                            MessageBox.Show("The username already exists. Please use a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Proceed to update user
                    string query = @"
        UPDATE users
        SET first_name = @first_name,
            middle_name = @middle_name,
            last_name = @last_name,
            username = @username,
            email = @email,
            password = @password,
            updated_at = NOW()
        WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@first_name", textBoxFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@middle_name", string.IsNullOrWhiteSpace(textBoxMiddleName.Text) ? DBNull.Value : (object)textBoxMiddleName.Text.Trim());
                        cmd.Parameters.AddWithValue("@last_name", textBoxLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@username", textBoxUserName.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", textBoxEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", textBoxPassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", UserId); // Ensure UserId is correctly set

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User info updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Users users = new Users();
                            users.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("No changes were made.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the user info:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
