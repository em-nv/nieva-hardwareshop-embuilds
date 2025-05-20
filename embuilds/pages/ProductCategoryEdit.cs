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
using System.Windows.Forms.VisualStyles;

namespace embuilds.pages
{
    public partial class ProductCategoryEdit : Form
    {
        conn_DB db = new conn_DB();
        public int ProductCategoryId { get; set; }
        public ProductCategoryEdit()
        {
            InitializeComponent();
        }

        private void frmProductCategoryEdit_Load(object sender, EventArgs e)
        {
            if (ProductCategoryId > 0)
            {
                ProductCategoryDetails();
            }
        }

        private void ProductCategoryDetails()
        {
            if (ProductCategoryId <= 0) return;

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT name, description
                FROM categories
                WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", ProductCategoryId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBoxCategoryName.Text = reader["name"].ToString();
                                textBoxDescription.Text = reader["description"].ToString();
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
                MessageBox.Show("An error occurred while loading category details:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ProductCategories productCategories = new ProductCategories();
            productCategories.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic input validation (description is optional)
            if (string.IsNullOrWhiteSpace(textBoxCategoryName.Text))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"
                UPDATE categories
                SET name = @name,
                    description = @description,
                    updated_at = NOW()
                WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBoxCategoryName.Text.Trim());
                        cmd.Parameters.AddWithValue("@description", textBoxDescription.Text.Trim()); // Optional field
                        cmd.Parameters.AddWithValue("@id", ProductCategoryId); // Ensure this variable exists and is set

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Category updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ProductCategories productCategories = new ProductCategories();
                            productCategories.Show();
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
                MessageBox.Show("An error occurred while saving the category:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
