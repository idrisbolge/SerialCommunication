﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectNo1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cable cb = new Cable();
            cb.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Wireless ws = new Wireless();
            ws.Show();
            this.Hide();
        }
    }
}
