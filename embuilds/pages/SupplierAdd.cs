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
    public partial class SupplierAdd : Form
    {
        conn_DB db = new conn_DB();
        public SupplierAdd()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Suppliers suppliers = new Suppliers();
            suppliers.Show();
            this.Hide();
        }

        private void SupplierAdd_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Get values from the textboxes
            string supplierName = textBoxSupplierName.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string contactPerson = textBoxContactPerson.Text.Trim();
            string contactNumber = textBoxNumber.Text.Trim();
            string address = textBoxAddress.Text.Trim();

            // Validate if required fields have values
            if (string.IsNullOrEmpty(supplierName))
            {
                MessageBox.Show("Supplier Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(contactPerson))
            {
                MessageBox.Show("Contact Person is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(contactNumber))
            {
                MessageBox.Show("Contact Number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

    
            // Proceed to insert the data into the suppliers table using the conn_DB class
            try
            {
                // Use the conn_DB class to get the database connection
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection()) // Assuming you have a method GetConnection() in conn_DB
                {

                    conn.Open(); // Ensure the connection is open before proceeding

                    MySqlTransaction transaction = conn.BeginTransaction();

                    string query = @"
                INSERT INTO suppliers (name, email, contact_person, contact_person_number, address, status, created_at, updated_at)
                VALUES (@name, @email, @contact_person, @contact_number, @address, @status, NOW(), NOW());";

                    MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
                    cmd.Parameters.AddWithValue("@name", supplierName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@contact_person", contactPerson);
                    cmd.Parameters.AddWithValue("@contact_number", contactNumber);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@status", "active");  // Assuming default status is 'Active'

                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Supplier saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally, reset input fields after saving
                    textBoxSupplierName.Clear();
                    textBoxEmail.Clear();
                    textBoxContactPerson.Clear();
                    textBoxNumber.Clear();
                    textBoxAddress.Clear();
                }

                Suppliers suppliers = new Suppliers();
                suppliers.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving supplier: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
