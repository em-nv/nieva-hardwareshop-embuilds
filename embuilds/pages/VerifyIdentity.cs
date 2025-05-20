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
    public partial class VerifyIdentity : Form
    {
        public int UserId { get; set; }
        public VerifyIdentity()
        {
            InitializeComponent();
        }

        private string userEmail;

        private void frmVerifyIdentity_Load(object sender, EventArgs e)
        {
            if (UserId > 0)
            {
                userDetails();
            }
        }

        private void userDetails()
        {
            if (UserId <= 0) return;

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT reset_question
                FROM users
                WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", UserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBoxResetQuestion.Text = reader["reset_question"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Show();
            this.Hide();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string answer = textBoxAnswer.Text.Trim();

            if (string.IsNullOrWhiteSpace(answer))
            {
                MessageBox.Show("Please enter your answer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT id
                FROM users
                WHERE id = @id AND reset_question = @question AND reset_answer = @answer
                LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", UserId);
                        cmd.Parameters.AddWithValue("@question", textBoxResetQuestion.Text.Trim());
                        cmd.Parameters.AddWithValue("@answer", answer);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            MessageBox.Show("Identity verified. Proceeding to password reset.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            ResetPassword resetPassword = new ResetPassword();
                            resetPassword.UserId = UserId;
                            resetPassword.Show();
                            this.Hide();



                            // Redirect to password reset form (example)
                            //PasswordReset resetForm = new PasswordReset();
                            //resetForm.UserId = UserId; // Pass user ID to next form
                            //resetForm.Show();
                            //this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect answer. Please try again.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during verification:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
