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
    public partial class CustomerAdd : Form
    {
        public CustomerAdd()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string firstName = textBoxFirstName.Text.Trim();
            string middleName = textBoxMiddleName.Text.Trim(); // Optional
            string lastName = textBoxLastName.Text.Trim();
            string phoneNumber = textBoxPhoneNumber.Text.Trim();
            string address = textBoxAddress.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(firstName))
            {
                MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Phone Number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidPhoneNumber(phoneNumber))
            {
                MessageBox.Show("Invalid Phone Number format. Please enter a 10-digit phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = new MySqlConnection(db.connectionString))
                {
                    conn.Open();

                    // Check if the phone number is already used by another customer
                    string phoneCheckQuery = "SELECT COUNT(*) FROM customers WHERE phone_number = @checkPhoneNumber";
                    using (MySqlCommand checkPhoneCmd = new MySqlCommand(phoneCheckQuery, conn))
                    {
                        checkPhoneCmd.Parameters.AddWithValue("@checkPhoneNumber", phoneNumber);

                        long phoneCount = (long)checkPhoneCmd.ExecuteScalar();

                        if (phoneCount > 0)
                        {
                            MessageBox.Show("The phone number already exists. Please use a different phone number.", "Duplicate Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Proceed to insert customer
                    string query = @"
        INSERT INTO customers 
            (first_name, middle_name, last_name, phone_number, address, created_at, updated_at)
        VALUES 
            (@firstName, @middleName, @lastName, @phoneNumber, @address, NOW(), NOW());";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@middleName", string.IsNullOrEmpty(middleName) ? DBNull.Value : (object)middleName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);
                        cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@address", address);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Customer saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally clear the fields
                    textBoxFirstName.Clear();
                    textBoxMiddleName.Clear();
                    textBoxLastName.Clear();
                    textBoxPhoneNumber.Clear();
                    textBoxAddress.Clear();
                }

                Customers customers = new Customers();
                customers.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to validate phone number format (example: 11 digits only)
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // A simple validation to check for a 11-digit phone number
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length == 11;
        }


        private void CustomerAdd_Load(object sender, EventArgs e)
        {

        }
    }
}
