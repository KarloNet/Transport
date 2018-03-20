using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; //obsluga sql

namespace TRANSPORT
{
    public partial class Form11 : Form
    {

        SqlConnection myCon;

        public Form11(SqlConnection con)
        {
            myCon = con;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fill;
            SqlCommand myCommand;

            if (textBox1.Text.Length > 1)
            {
                fill = "INSERT INTO USERS VALUES ('" + textBox1.Text + "')";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad6)
                {
                    MessageBox.Show(blad6.ToString());
                }
                Close();
            }
            else
            {
                MessageBox.Show("Wprowadź nazwę operatora !");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
