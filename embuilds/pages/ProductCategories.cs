using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace embuilds.pages
{
    public partial class ProductCategories : Form
    {
        public ProductCategories()
        {
            InitializeComponent();
        }

        private void frmProductCategories_Load(object sender, EventArgs e)
        {
            var conn_db = new conn_DB();
            dataGridProductCategories.DataSource = conn_db.GetAllProductCategories();
            dataGridProductCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }
    }
}
