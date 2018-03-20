using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Collections;

namespace TRANSPORT
{
    public partial class Form6 : Form
    {
        SqlConnection myCon;
        ArrayList myList;

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            string file;
            StreamReader read;

            try
            {
                read = new StreamReader("setup.ini");
            }
            catch (FileNotFoundException)
            {
                read = null;
            }

            if (read != null)
            {
                file = read.ReadToEnd();
                read.Close();
                myCon = new SqlConnection(file);
                //MessageBox.Show("Poprawnie wczytano plik system.ini \n Połączono z bazą SQL : \n"+ file);
            }
            else
            {
                myCon = new SqlConnection("server=(local)\\SQLEXPRESS;database=main;Integrated Security=SSPI");
            }

            if (myCon != null)
            {
                try
                {
                    myCon.Open();
                }
                catch (Exception blad3)
                {
                    MessageBox.Show("Nie udało się nawiązać połaczenia z SQL \n" + blad3.ToString());
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Nie udało się nawiązać połaczenia z SQL");
                Close();
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            string fill;
            string dataStreem;
            string dataStreem2;
            SqlDataReader myReader;
            SqlCommand myCommand;
            myList = new ArrayList();

            comboBox1.Items.Clear();                            //czysci na wszelki wypadek dane w boxie
            fill = "SELECT Turnus, Od, Do FROM TURNUSY";
            try
            {

                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();


                while (myReader.Read())
                {

                    dataStreem = (string)myReader["Turnus"];
                    comboBox1.Items.Add(dataStreem);
                    dataStreem = (string)myReader["Od"];
                    dataStreem2 = (string)myReader["Do"];
                    myList.Add( (dataStreem.Trim() + " - " + dataStreem2.Trim()) );
                }
                myReader.Close();
            }
            catch (Exception blad1)
            {
                MessageBox.Show(blad1.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (myList.Count >= comboBox1.SelectedIndex)
            {
                label2.Text = myList[comboBox1.SelectedIndex].ToString();
            }
        }

    }
}
