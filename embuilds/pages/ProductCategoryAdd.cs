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
    public partial class ProductCategoryAdd : Form
    {
        public ProductCategoryAdd()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ProductCategories productCategories = new ProductCategories();
            productCategories.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string categoryName = textBoxCategoryName.Text.Trim();
            string description = textBoxDescription.Text.Trim();

            // Validate that category name is not empty
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conn_DB db = new conn_DB();
                using (MySqlConnection conn = new MySqlConnection(db.connectionString))
                {
                    conn.Open();

                    string query = @"
                INSERT INTO categories (name, description, created_at, updated_at)
                VALUES (@name, @description, NOW(), NOW());";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", categoryName);
                        cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(description) ? DBNull.Value : (object)description);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Category saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally clear inputs
                    textBoxCategoryName.Clear();
                    textBoxDescription.Clear();
                }
                ProductCategories productCategories = new ProductCategories();
                productCategories.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving category: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
