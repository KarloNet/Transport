using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; //obsluga sql
using System.IO; //wejscie /wyjscie -> pliki


namespace TRANSPORT
{
    public partial class Form3 : Form
    {

        SqlConnection myCon;

        public event ValueUpdatedEventHandler ValueUpdated;

        public Form3(SqlConnection con)
        {
            InitializeComponent();
            myCon = con;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            FillBoxUsers();
            FillBoxBase();

            string file = "";
            StreamReader read;

            // wczytujemy domyslnego operatora
            try
            {
                read = new StreamReader("user.ini");
            }
            catch (FileNotFoundException)
            {
                read = null;
            }
            
            if (read != null)
            {
                file = read.ReadToEnd();
                read.Close();
                int i = 0;
                i = comboBox1.FindStringExact(file, 0);
                comboBox1.SelectedIndex = i;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }

            //wczytujemy domyslna baze
            try
            {
                read = new StreamReader("base.ini");
            }
            catch (FileNotFoundException)
            {
                read = null;
            }

            if (read != null)
            {
                file = read.ReadToEnd();
                read.Close();
                int i = 0;
                i = comboBox2.FindStringExact(file, 0);
                comboBox2.SelectedIndex = i;
            }
            else
            {
                comboBox2.SelectedIndex = 0;
            }
        }

        private void FillBoxUsers()
        {
            string fill;
            string dataStreem;

            SqlDataReader myReader;
            SqlCommand myCommand;

            comboBox1.Items.Clear(); //czysci na wszelki wypadek dane w boxie

            fill = "SELECT * FROM USERS";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["User"];
                    comboBox1.Items.Add(dataStreem);
                }
                myReader.Close();
            }
            catch (Exception blad6)
            {
                MessageBox.Show(blad6.ToString());
            }
        }

        private void FillBoxBase()
        {
            string fill;
            string dataStreem;

            SqlDataReader myReader;
            SqlCommand myCommand;

            comboBox2.Items.Clear(); //czysci na wszelki wypadek dane w boxie
            
            fill = "SELECT * FROM DBASE";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["DB"];
                    comboBox2.Items.Add(dataStreem);
                }
                myReader.Close();
            }
            catch (Exception blad6)
            {
                MessageBox.Show(blad6.ToString());
            }
            
        }
        private void UsunOperatora()
        {
            string fill;
            SqlCommand myCommand;
            int i = 0;
            if (comboBox1.Text != "")
            {
                fill = "DELETE FROM USERS WHERE User = '" + comboBox1.Text.Trim() + "'";
                
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    i= myCommand.ExecuteNonQuery();
                }
                catch (Exception blad6)
                {
                    MessageBox.Show(blad6.ToString());
                }
                MessageBox.Show(fill + "\n" + myCon.ConnectionString + "\n" + i.ToString());
                FillBoxUsers();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamWriter write;

            if (comboBox1.Text != "")
            {
                string newValue = this.comboBox1.Text;
                Dane tmp = new Dane();
                tmp.stringVal = newValue;
                tmp.val = 3;
                ValueUpdatedEventArgs valueArgs = new ValueUpdatedEventArgs(tmp);
                ValueUpdated(this, valueArgs);

                try
                {
                    write = new StreamWriter("user.ini");
                }
                catch (FileNotFoundException)
                {
                    write = null;
                }

                if (write != null)
                {
                    write.Write(comboBox1.Text);
                    write.Close();
                }
            }
            else
            {
                string newValue = "close";
                Dane tmp = new Dane();
                tmp.stringVal = newValue;
                tmp.val = 3;
                ValueUpdatedEventArgs valueArgs = new ValueUpdatedEventArgs(tmp);
                ValueUpdated(this, valueArgs);
            }

            if (comboBox2.Text != "")
            {
                string newValue = this.comboBox2.Text;
                Dane tmp = new Dane();
                tmp.stringVal = newValue;
                tmp.val = 4;
                ValueUpdatedEventArgs valueArgs = new ValueUpdatedEventArgs(tmp);
                ValueUpdated(this, valueArgs);

                try
                {
                    write = new StreamWriter("base.ini");
                }
                catch (FileNotFoundException)
                {
                    write = null;
                }

                if (write != null)
                {
                    write.Write(comboBox2.Text);
                    write.Close();
                }
            }

            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
                string newValue = "close";
                Dane tmp = new Dane();
                tmp.stringVal = newValue;
                tmp.val = 3;
                ValueUpdatedEventArgs valueArgs = new ValueUpdatedEventArgs(tmp);
                ValueUpdated(this, valueArgs);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form11 dlg = new Form11(myCon);
            dlg.ShowDialog();
            FillBoxUsers();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Usunąć operatora ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UsunOperatora();
            }
        }


    }
}
