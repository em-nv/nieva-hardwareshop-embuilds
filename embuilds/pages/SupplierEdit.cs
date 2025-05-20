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
    public partial class SupplierEdit : Form
    {
        conn_DB db = new conn_DB();
        public int SupplierId { get; set; }
        public SupplierEdit()
        {
            InitializeComponent();
        }

        private void frmSupplierEdit_Load(object sender, EventArgs e)
        {
            // Populate status combo box
            comboBoxStatus.Items.Clear();
            comboBoxStatus.Items.Add("active");
            comboBoxStatus.Items.Add("inactive");
            comboBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            // Optionally set a default value
            comboBoxStatus.SelectedIndex = 0;


            if (SupplierId > 0)
            {
                LoadSupplierDetails();
            }
        }

        private void LoadSupplierDetails()
        {
            if (SupplierId <= 0) return;

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT name, email, contact_person, contact_person_number, address, status
                FROM suppliers
                WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", SupplierId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBoxSupplierName.Text = reader["name"].ToString();
                                textBoxEmail.Text = reader["email"].ToString();
                                textBoxContactPerson.Text = reader["contact_person"].ToString();
                                textBoxNumber.Text = reader["contact_person_number"].ToString();
                                textBoxAddress.Text = reader["address"].ToString();
                                comboBoxStatus.SelectedItem = reader["status"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Supplier not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading supplier details:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            Suppliers suppliers = new Suppliers();
            suppliers.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic input validation
            if (string.IsNullOrWhiteSpace(textBoxSupplierName.Text) ||
                string.IsNullOrWhiteSpace(textBoxEmail.Text) ||
                string.IsNullOrWhiteSpace(textBoxContactPerson.Text) ||
                string.IsNullOrWhiteSpace(textBoxNumber.Text) ||
                string.IsNullOrWhiteSpace(textBoxAddress.Text) ||
                comboBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                UPDATE suppliers
                SET name = @name,
                    email = @email,
                    contact_person = @contact_person,
                    contact_person_number = @contact_number,
                    address = @address,
                    status = @status,
                    updated_at = NOW()
                WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBoxSupplierName.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", textBoxEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@contact_person", textBoxContactPerson.Text.Trim());
                        cmd.Parameters.AddWithValue("@contact_number", textBoxNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", textBoxAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@status", comboBoxStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@id", SupplierId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Suppliers suppliers = new Suppliers();
                            suppliers.Show();
                            this.Close();
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
                MessageBox.Show("An error occurred while saving the supplier:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
