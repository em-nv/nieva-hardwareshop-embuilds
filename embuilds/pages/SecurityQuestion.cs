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
    public partial class SecurityQuestion : Form
    {
        private int userId;
        private conn_DB db;

        public SecurityQuestion(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.db = new conn_DB();
            comboBoxSecurityQuestions.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MyAccount myAccount = new MyAccount(userId);
            myAccount.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboBoxSecurityQuestions.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a security question",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxAnswer.Text))
            {
                MessageBox.Show("Please enter an answer",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string question = comboBoxSecurityQuestions.SelectedItem.ToString();
            string answer = textBoxAnswer.Text.Trim();

            using (MySqlConnection conn = db.GetConnection())
            {
                string query = @"UPDATE users 
                            SET reset_question = @question, 
                                reset_answer = @answer 
                            WHERE id = @userId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@question", question);
                cmd.Parameters.AddWithValue("@answer", answer);
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Security question updated successfully!",
                                      "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Return to MyAccount form
                        MyAccount myAccount = new MyAccount(userId);
                        myAccount.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving security question: " + ex.Message,
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmSecurityQuestion_Load(object sender, EventArgs e)
        {
            // Initialize combo box with security question options
            comboBoxSecurityQuestions.Items.AddRange(new string[] {
            "First Pet's Name",
            "Favorite Food",
            "Favorite Place"
        });

            // Load current security question if exists
            LoadCurrentSecurityQuestion();
        }
        private void LoadCurrentSecurityQuestion()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                string query = "SELECT reset_question, reset_answer FROM users WHERE id = @userId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string currentQuestion = reader["reset_question"].ToString();
                            string currentAnswer = reader["reset_answer"].ToString();

                            if (!string.IsNullOrEmpty(currentQuestion))
                            {
                                comboBoxSecurityQuestions.Text = currentQuestion;
                                textBoxAnswer.Text = currentAnswer;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading security question: " + ex.Message,
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
