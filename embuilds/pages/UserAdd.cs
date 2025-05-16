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

namespace embuilds.pages
{
    public partial class UserAdd : Form
    {
        public UserAdd()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string firstName = textBoxFirstName.Text.Trim();
            string middleName = textBoxMiddleName.Text.Trim(); // Optional
            string lastName = textBoxLastName.Text.Trim();
            string username = textBoxUserName.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            // Validate required fields
            if (string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("All fields except Middle Name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = new MySqlConnection(db.connectionString))
                {
                    conn.Open();

                    // Check if email already exists
                    string checkEmailQuery = "SELECT COUNT(*) FROM users WHERE email = @checkEmail";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkEmailQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@checkEmail", email);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("The email already exists. Please use a different email.", "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Check if username already exists
                    string checkUsernameQuery = "SELECT COUNT(*) FROM users WHERE username = @checkUsername";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkUsernameQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@checkUsername", username);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("The username already exists. Please use a different username.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert new user
                    string insertQuery = @"
        INSERT INTO users 
            (first_name, middle_name, last_name, username, email, password, reset_question, reset_answer, created_at, updated_at)
        VALUES 
            (@firstName, @middleName, @lastName, @username, @email, @password, NULL, NULL, NOW(), NOW());";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@middleName", string.IsNullOrEmpty(middleName) ? DBNull.Value : (object)middleName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", password);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("User saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally clear the fields
                    textBoxFirstName.Clear();
                    textBoxMiddleName.Clear();
                    textBoxLastName.Clear();
                    textBoxUserName.Clear();
                    textBoxEmail.Clear();
                    textBoxPassword.Clear();
                }

                Users users = new Users();
                users.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void UserAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
