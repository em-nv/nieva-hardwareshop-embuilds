﻿using System;
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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        //private void Products_Click(object sender, EventArgs e)
        //{
        //    Products products = new Products();
        //    products.Show();
        //    this.Hide();
        //}

        private void btnInventory_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory();
            inventory.Show();
            this.Hide();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            products.Show();
            this.Hide();
        }

        private void btnPOS_click(object sender, EventArgs e)
        {
            POS pos = new POS();
            pos.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private Timer dateTimeTimer;
        private void Dashboard_Load(object sender, EventArgs e)
        {
            dateTimeTimer = new Timer();
            dateTimeTimer.Interval = 1000; // 1 second
            dateTimeTimer.Tick += UpdateDateTime;
            dateTimeTimer.Start();
        }

        private void UpdateDateTime(object sender, EventArgs e)
        {
            dateAndTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void buttonSuppliers_Click(object sender, EventArgs e)
        {
            Suppliers suppliers = new Suppliers();
            suppliers.Show();
            this.Hide();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            Sales sales = new Sales();
            sales.Show();
            this.Hide();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            customers.Show();
            this.Hide();
        }

        private void dashboardLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            Transactions transactions = new Transactions();
            transactions.Show();
            this.Hide();
        }

        private void btnProductCategories_Click(object sender, EventArgs e)
        {
            ProductCategories productCategories = new ProductCategories();
            productCategories.Show();
            this.Hide();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Show();
            this.Hide();
        }

        private void btnManageAccount_Click(object sender, EventArgs e)
        {
            MyAccount myAccount = new MyAccount();
            myAccount.Show();
            this.Hide();
        }
    }
}
