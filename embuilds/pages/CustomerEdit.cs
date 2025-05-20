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
                string.IsNullOrWhiteSpace(textBoxAddress.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string phoneNumber = textBoxPhoneNumber.Text.Trim();

            // Mobile number validation
            if (!IsValidMobileNumber(phoneNumber))
            {
                MessageBox.Show("Invalid mobile number. Please enter a valid 10-digit mobile number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    // Check if the mobile number already exists (except for the current customer)
                    string phoneCheckQuery = "SELECT COUNT(*) FROM customers WHERE phone_number = @checkPhoneNumber AND id != @currentId";
                    using (MySqlCommand checkPhoneCmd = new MySqlCommand(phoneCheckQuery, conn))
                    {
                        checkPhoneCmd.Parameters.AddWithValue("@checkPhoneNumber", phoneNumber);
                        checkPhoneCmd.Parameters.AddWithValue("@currentId", CustomerId);  // Ensure CustomerId is set

                        long phoneCount = (long)checkPhoneCmd.ExecuteScalar();

                        if (phoneCount > 0)
                        {
                            MessageBox.Show("The mobile number already exists. Please use a different mobile number.", "Duplicate Mobile Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Proceed to update customer
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
                        cmd.Parameters.AddWithValue("@phone_number", phoneNumber);
                        cmd.Parameters.AddWithValue("@address", textBoxAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", CustomerId); // Ensure CustomerId is defined and set

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

        // Helper method to validate mobile number format (example: 11 digits only)
        private bool IsValidMobileNumber(string phoneNumber)
        {
            // Check if the phone number contains only digits and is exactly 11 digits long
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length == 11;
        }


    }
}
