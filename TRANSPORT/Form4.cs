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

namespace TRANSPORT
{
    public partial class Form4 : Form
    {

        SqlConnection myCon = null;
        //bool zmiany = false;

        public Form4(SqlConnection con)
        {
            InitializeComponent();
            myCon = con;
        }

        //sprawdza czy podany zleceniodawca istnieje w bazie 'glowna'
        private bool IstniejeZleceniodawca(string strZlec, string strTurnus)
        {
            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem = "";
            string fill;
            int count = 0;

            fill = "SELECT Zleceniodawca FROM GLOWNA WHERE Zleceniodawca ='" + strZlec + "' AND Turnus = '" + strTurnus + "'";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["Zleceniodawca"];
                    count++;
                }
                myReader.Close();
            }
            catch (Exception blad4)
            {
                MessageBox.Show(blad4.ToString());
            }


            if (dataStreem != "")
            {
                return true;
                //MessageBox.Show(" Znaleziono cosik w ilosci :" + count.ToString());
            }
            else return false;

        }


        private void Form4_Load(object sender, EventArgs e)
        {

            if (myCon.State != ConnectionState.Open)
            {
                MessageBox.Show("ERROR\nConnection is closed");
                Close();
            }
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            string fill;
            string dataStreem;
            SqlDataReader myReader;
            SqlCommand myCommand;

            comboBox1.Items.Clear();                            //czysci na wszelki wypadek dane w boxie
            fill = "SELECT Turnus FROM TURNUSY";
            try
            {

                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();


                while (myReader.Read())
                {

                    dataStreem = (string)myReader["Turnus"];
                    comboBox1.Items.Add(dataStreem);
                }
                myReader.Close();
            }
            catch (Exception blad1)
            {
                MessageBox.Show(blad1.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //zmiany = true;
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand myCommand;

            if ((textBox1.Text.Trim() != "") && (numericUpDown1.Value != 0) && (comboBox1.Text.Trim() != ""))
            {
                if (IstniejeZleceniodawca(textBox1.Text, comboBox1.Text))
                {
                    MessageBox.Show("Podany zleceniodawca juz jest w bazie\nNie można dodać!");
                }
                else
                {
                    try
                    {
                        myCommand = new SqlCommand("INSERT INTO GLOWNA ( Turnus, Zleceniodawca, Kod_pocz, Miejscowosc, Ul, Nr_lokalu, Os_kontaktowa, Tel_kontaktowy, Il_dzieci, Wyjazd, Powrot, Uwagi, Stow, Organizator) VALUES ( '" + comboBox1.Text + "','" + textBox1.Text + "','" + textBox7.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + textBox8.Text + "','" + textBox9.Text + "'," + numericUpDown1.Value + ",'" + textBox10.Text + "','" + textBox2.Text + "','" + textBox11.Text + "',0,'" + textBox3.Text + "')", myCon);
                        myCommand.ExecuteNonQuery();
                    }
                    catch (Exception blad)
                    {
                        MessageBox.Show(blad.ToString());
                    }
                    MessageBox.Show("Dodano nowego zleceniodawce");
                    button1.DialogResult = DialogResult.OK;
                    Close();

                }
            }
            else
            {
                MessageBox.Show(" Musi byc wprowadzony zleceniodawca i wybrany turnus");
            }


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
