using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MySql.Data.MySqlClient;

namespace embuilds.pages
{
    public partial class CustomerEdit : Form
    {
        conn_DB db = new conn_DB();
        public int CustomerId { get; set; }
        public CustomerEdit()
        {
            InitializeComponent();
        }

        private void frmCustomerEdit_Load(object sender, EventArgs e)
        {
            if (CustomerId > 0)
            {
                CustomerDetails();
            }
        }

        private void CustomerDetails()
        {
            if (CustomerId <= 0) return;

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT id, first_name, middle_name, last_name, phone_number, address
                FROM customers
                WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", CustomerId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBoxFirstName.Text = reader["first_name"].ToString();
                                textBoxMiddleName.Text = reader["middle_name"].ToString();
                                textBoxLastName.Text = reader["last_name"].ToString();
                                textBoxPhoneNumber.Text = reader["phone_number"].ToString();
                                textBoxAddress.Text = reader["address"].ToString();

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
            Customers customers = new Customers();
            customers.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic input validation (middle name is optional)
            if (string.IsNullOrWhiteSpace(textBoxFirstName.Text) ||
                string.IsNullOrWhiteSpace(textBoxLastName.Text) ||
                string.IsNullOrWhiteSpace(textBoxPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(textBoxAddress.Text)) // fixed closing parenthesis
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

                    string query = @"
                UPDATE customers
                SET first_name = @first_name,
                    middle_name = @middle_name,
                    last_name = @last_name,
                    phone_number = @phone_number,
                    address = @address,
                    updated_at = NOW()
                WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@first_name", textBoxFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@middle_name", string.IsNullOrWhiteSpace(textBoxMiddleName.Text) ? DBNull.Value : (object)textBoxMiddleName.Text.Trim());
                        cmd.Parameters.AddWithValue("@last_name", textBoxLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone_number", textBoxPhoneNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", textBoxAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", CustomerId); // Make sure UserId is defined and set

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Customers customers = new Customers();
                            customers.Show();
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
                MessageBox.Show("An error occurred while saving the customer details:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
