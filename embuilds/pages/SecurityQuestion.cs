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
    public partial class SecurityQuestion : Form
    {
        public SecurityQuestion()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MyAccount myAccount = new MyAccount();
            myAccount.Show();
            this.Hide();
        }
    }
}
