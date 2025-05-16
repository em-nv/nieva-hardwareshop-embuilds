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
    public partial class MyAccount : Form
    {
        private int userId;
        private conn_DB db;

        public MyAccount(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.db = new conn_DB(); // Initialize your DB connection helper
        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard(userId); // Pass the userId back
            dashboard.Show();
            this.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SecurityQuestion securityQuestionForm = new SecurityQuestion(userId);
            securityQuestionForm.Show();
            this.Hide();
        }

        private void MyAccount_Load(object sender, EventArgs e)
        {
            
        }

        private void frmMyAccount_Load(object sender, EventArgs e)
        {
            LoadUserData();

            // Make controls non-editable and unclickable on load
            firstNameInput.Enabled = false;
            middleNameInput.Enabled = false;
            lastNameInput.Enabled = false;
            usernameInput.Enabled = false;
            emailInput.Enabled = false;

            securityQuestion.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
        }
        private void LoadUserData()
        {
            using (MySqlConnection conn = db.GetConnection()) // Use your existing connection method
            {
                string query = @"SELECT first_name, middle_name, last_name, 
                                   username, email, reset_question 
                            FROM users 
                            WHERE id = @userId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            firstNameInput.Text = reader["first_name"].ToString();
                            middleNameInput.Text = reader["middle_name"].ToString();
                            lastNameInput.Text = reader["last_name"].ToString();
                            usernameInput.Text = reader["username"].ToString();
                            emailInput.Text = reader["email"].ToString();

                            string resetQuestion = reader["reset_question"].ToString();
                            securityQuestion.Text = string.IsNullOrEmpty(resetQuestion) ? "None" : resetQuestion;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading user data: " + ex.Message, "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditProfileInfo_Click(object sender, EventArgs e)
        {
            // Make controls editable and clickable
            firstNameInput.Enabled = true;
            middleNameInput.Enabled = true;
            lastNameInput.Enabled = true;
            usernameInput.Enabled = true;
            emailInput.Enabled = true;

            btnEdit.Enabled = true;
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Get values from form controls
            string firstName = firstNameInput.Text.Trim();
            string middleName = middleNameInput.Text.Trim();
            string lastName = lastNameInput.Text.Trim();
            string username = usernameInput.Text.Trim();
            string email = emailInput.Text.Trim();

            // Validate required fields
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please fill in all required fields",
                              "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate email format
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                MessageBox.Show("Please enter a valid email address",
                              "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Check if username already exists for another user
                    string checkUserQuery = @"SELECT COUNT(*) FROM users 
                                    WHERE username = @username AND id != @userId";
                    MySqlCommand checkUserCmd = new MySqlCommand(checkUserQuery, conn);
                    checkUserCmd.Parameters.AddWithValue("@username", username);
                    checkUserCmd.Parameters.AddWithValue("@userId", userId);

                    int userCount = Convert.ToInt32(checkUserCmd.ExecuteScalar());
                    if (userCount > 0)
                    {
                        MessageBox.Show("Username already exists. Please choose a different one.",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Check if email already exists for another user
                    string checkEmailQuery = @"SELECT COUNT(*) FROM users 
                                      WHERE email = @email AND id != @userId";
                    MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailQuery, conn);
                    checkEmailCmd.Parameters.AddWithValue("@email", email);
                    checkEmailCmd.Parameters.AddWithValue("@userId", userId);

                    int emailCount = Convert.ToInt32(checkEmailCmd.ExecuteScalar());
                    if (emailCount > 0)
                    {
                        MessageBox.Show("Email already exists. Please use a different one.",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update user information
                    string updateQuery = @"UPDATE users 
                                SET first_name = @firstName,
                                    middle_name = @middleName,
                                    last_name = @lastName,
                                    username = @username,
                                    email = @email
                                WHERE id = @userId";

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@firstName", firstName);
                    updateCmd.Parameters.AddWithValue("@middleName", string.IsNullOrWhiteSpace(middleName) ? (object)DBNull.Value : middleName);
                    updateCmd.Parameters.AddWithValue("@lastName", lastName);
                    updateCmd.Parameters.AddWithValue("@username", username);
                    updateCmd.Parameters.AddWithValue("@email", email);
                    updateCmd.Parameters.AddWithValue("@userId", userId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile updated successfully!",
                                      "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Make fields read-only again after saving
                        firstNameInput.ReadOnly = true;
                        middleNameInput.ReadOnly = true;
                        lastNameInput.ReadOnly = true;
                        usernameInput.ReadOnly = true;
                        emailInput.ReadOnly = true;
                        btnEdit.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("No changes were made to your profile.",
                                      "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating profile: " + ex.Message,
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
