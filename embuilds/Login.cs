using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using embuilds.pages;



namespace embuilds
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            conn_DB db = new conn_DB();
            int userId = db.ValidateLogin(username, password);

            if (userId > 0)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Dashboard dashboard = new Dashboard(userId); // Pass the user ID
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Display confirmation dialog only when the user tries to close the form manually
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var answer = MessageBox.Show("Do you want to close the application?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Check the user's response
                if (answer == DialogResult.Yes)
                {
                    // User chose Yes, proceed with closing the application
                    Application.Exit();
                }
                else
                {
                    // User chose No, cancel the close action
                    e.Cancel = true;
                }
            }
        }

        private void linkLabel1_ForgotPasswordClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassword forgotPasswordForm = new ForgotPassword();
            forgotPasswordForm.Show();
            this.Hide();
        }
    }
}
