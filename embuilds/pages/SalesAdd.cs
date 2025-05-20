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
    public partial class SalesAdd : Form
    {
        int rowIndex = 0;
        int currentY = 150;
        int rowHeight = 30;
        public SalesAdd()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void SalesAdd_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Assuming your inner TableLayoutPanel is named tableLayoutPanel
            var table = tableLayoutPanel;

            // Increase the row count
            table.RowCount++;
            int row = table.RowCount - 1;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Create controls
            TextBox textBoxProductName = new TextBox();
            TextBox textBoxUnitPrice = new TextBox();
            TextBox textBoxQuantity = new TextBox();
            TextBox textBoxTotalPrice = new TextBox();
            Button btnRemove = new Button();

            // Configure controls
            textBoxProductName.Width = 150;
            textBoxUnitPrice.Width = 100;
            textBoxQuantity.Width = 100;
            textBoxTotalPrice.Width = 120;
            textBoxTotalPrice.ReadOnly = true;

            btnRemove.Text = "Remove";
            btnRemove.Click += (s, args) =>
            {
                // Remove all controls from this row
                for (int i = 0; i < table.ColumnCount; i++)
                {
                    var control = table.GetControlFromPosition(i, row);
                    if (control != null)
                        table.Controls.Remove(control);
                }
            };

            // Add controls to the new row
            table.Controls.Add(textBoxProductName, 0, row);
            table.Controls.Add(textBoxUnitPrice, 1, row);
            table.Controls.Add(textBoxQuantity, 2, row);
            table.Controls.Add(textBoxTotalPrice, 3, row);
            table.Controls.Add(btnRemove, 4, row);
        }

        private void tableLayoutPanelRows_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
