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
using System.Drawing.Printing; // do drukowania


namespace TRANSPORT
{
    public partial class Form7 : Form
    {

        private SqlConnection myCon;
        private string zlecenie;
        private string turnus;
        private string wybranyOperator;
        private PrintDocument printDoc; // zmienna do drukowania
        private string zmiany;
        private bool zmieniono = false;
        private bool[] zmiany_bool = new bool[12];

        public Form7(SqlConnection con, string strZlec, string strTurnus, string wybOperator)
        {
            myCon = con;
            zlecenie = strZlec;
            turnus = strTurnus;
            wybranyOperator = wybOperator;
            InitializeComponent();
            for (int i = 0; i < 12; i++)
            {
                zmiany_bool[i] = false;
            }
                printDoc = new PrintDocument(); //ustawia zmienna globalna do drukowania
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage); //wiąze kazde wywołanie print z funkcją printDoc_PrintPage

        }

        private void Form7_Load(object sender, EventArgs e)
        {


            if (myCon.State != ConnectionState.Open)
            {
                MessageBox.Show("ERROR\nConnection closed can't read data\n");
                Close();
            }
            FillUp();
        }

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (zmieniono)
            {
                if (MessageBox.Show("Zapisać zmiany?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ZapiszZmiany();
                }
            }
        }

        // wyświetla wszystkie dane zleceniodawcy + combobox wychowawcy + combobox turnusy
        private void FillUp()
        {

            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string dataStreem;
            string dataStreem2;
            bool powiadomiono = false;
            char tab = '\u0009';


            // wybrano oba warunki w turnus i zleceniodawca
            fill = "SELECT Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA WHERE Turnus = '" + turnus + "' AND Zleceniodawca ='" + zlecenie + "'";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {


                    dataStreem = Convert.ToString(myReader["Il_dzieci"]);    // ilosc dzieci
                    textBox8.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Os_kontaktowa"];      // osoba kontaktowa
                    textBox9.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Tel_kontaktowy"];      // telefon do os. kontaktowej
                    textBox10.Text = dataStreem.Trim();

                    dataStreem = (string)myReader["Ul"];      // telefon do os. kontaktowej
                    textBox4.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Kod_pocz"];      // telefon do os. kontaktowej
                    textBox11.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Miejscowosc"];      // telefon do os. kontaktowej
                    textBox12.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Nr_lokalu"];      // telefon do os. kontaktowej
                    textBox13.Text = dataStreem.Trim();

                    powiadomiono = (bool)myReader["Powiadomiono"];
                    if (powiadomiono)
                    {
                        textBox16.Text = ".......";// dataStreem.Trim();
                        dataStreem = (string)myReader["Data_powiadom"];      // telefon do os. kontaktowej
                        textBox17.Text = dataStreem.Trim();
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        textBox16.Clear();
                        textBox17.Clear();
                        checkBox1.Checked = false;
                    }
                    textBox1.Clear();
                    textBox3.Clear();
                    if (myReader["Wyjazd"] != DBNull.Value)
                    {
                        dataStreem = (string)myReader["Wyjazd"];
                        dataStreem = dataStreem.Replace(tab.ToString(),"");
                        
                        char[] temp = dataStreem.ToCharArray();
                        for (int i = 0; i < temp.Length; i++)
                        {
                            //Kasuje zdublowanie numeru pociagu osobowego
                            if ((temp[i] == 'O') && (i + 6 < temp.Length) )
                                if ((temp[i + 1] == 's') && (temp[i + 2] == 'o') && (temp[i + 3] == 'b') && (temp[i + 4] == 'o') && (temp[i + 5] == 'w') && (temp[i + 6] == 'y'))
                                {
                                    int t = i - 3;
                                    while ( (t > 0) && ( (int)temp[t] > 47) && ( (int)temp[t] < 58) )
                                    {
                                        temp[t] = '|';
                                        t--;
                                    }
                                    if( (int)temp[i-1] == 10 ) temp[i - 1] = '|';
                                    if( (int)temp[i-2] == 13 ) temp[i - 2] = '|';
                                    //t++;
                                    //dataStreem = dataStreem.Remove(t, (i-t) );
                                }
                            //Kasuje zdublowanie numeru pociagu pospiesznego
                            if ((temp[i] == ' ') && (i + 7 < temp.Length) && ( temp[i+1] == 'D'))
                                if ((temp[i + 2] == ' ') && ((int)temp[i + 3] > 47) && ((int)temp[i + 3] < 58) && ((int)temp[i + 4] > 47) && ((int)temp[i + 4] < 58) && ((int)temp[i + 5] > 47) && ((int)temp[i + 5] < 58) && ((int)temp[i + 6] > 47) && ((int)temp[i + 6] < 58) && ((int)temp[i + 7] > 47) && ((int)temp[i + 7] < 58))
                                {
                                    int t = i + 3;
                                    while ((t < temp.Length) && ((int)temp[t] > 47) && ((int)temp[t] < 58))
                                    {
                                        temp[t] = '|';
                                        t++;
                                    }
                                    if (t + 1 < temp.Length)
                                    {
                                        if ((int)temp[t] == 13) temp[t] = '|';
                                        if ((int)temp[t + 1] == 10) temp[t + 1] = '|';
                                    }
                                    temp[i+1] = '|';
                                    temp[i + 2] = '|';
                                    //dataStreem = dataStreem.Remove(t, (i - t));
                                }
                        }
                        dataStreem = new string(temp);
                        
                            dataStreem = dataStreem.Replace("|","");
                            textBox1.Text = dataStreem;
                        //textBox1.Text = (string)myReader["Wyjazd"];
                    }
                    if (myReader["Powrot"] != DBNull.Value)
                    {
                        dataStreem = (string)myReader["Powrot"];
                        dataStreem = dataStreem.Replace(tab.ToString(),"");

                        char[] temp = dataStreem.ToCharArray();
                        for (int i = 0; i < temp.Length; i++)
                        {
                            //Kasuje zdublowanie numeru pociagu osobowego
                            if ((temp[i] == 'O') && (i + 6 < temp.Length))
                                if ((temp[i + 1] == 's') && (temp[i + 2] == 'o') && (temp[i + 3] == 'b') && (temp[i + 4] == 'o') && (temp[i + 5] == 'w') && (temp[i + 6] == 'y'))
                                {
                                    int t = i - 3;
                                    while ((t > 0) && ((int)temp[t] > 47) && ((int)temp[t] < 58))
                                    {
                                        temp[t] = '|';
                                        t--;
                                    }
                                    if ((int)temp[i - 1] == 10) temp[i - 1] = '|';
                                    if ((int)temp[i - 2] == 13) temp[i - 2] = '|';
                                    //t++;
                                    //dataStreem = dataStreem.Remove(t, (i-t) );
                                }
                            //Kasuje zdublowanie numeru pociagu pospiesznego
                            if ((temp[i] == ' ') && (i + 7 < temp.Length) && (temp[i + 1] == 'D'))
                                if ((temp[i + 2] == ' ') && ((int)temp[i + 3] > 47) && ((int)temp[i + 3] < 58) && ((int)temp[i + 4] > 47) && ((int)temp[i + 4] < 58) && ((int)temp[i + 5] > 47) && ((int)temp[i + 5] < 58) && ((int)temp[i + 6] > 47) && ((int)temp[i + 6] < 58) && ((int)temp[i + 7] > 47) && ((int)temp[i + 7] < 58))
                                {
                                    int t = i + 3;
                                    while ((t < temp.Length) && ((int)temp[t] > 47) && ((int)temp[t] < 58))
                                    {
                                        temp[t] = '|';
                                        t++;
                                    }
                                    if (t + 1 < temp.Length)
                                    {
                                        if ((int)temp[t] == 13) temp[t] = '|';
                                        if ((int)temp[t + 1] == 10) temp[t + 1] = '|';
                                    }
                                    temp[i + 1] = '|';
                                    temp[i + 2] = '|';
                                    //dataStreem = dataStreem.Remove(t, (i - t));
                                }
                        }
                        dataStreem = new string(temp);
                        dataStreem = dataStreem.Replace("|", "");

                        textBox3.Text = dataStreem;
                    }
                    textBox2.Clear();
                    textBox2.Text = (string)myReader["Uwagi"];
                    textBox18.Clear();
                    if (myReader["Organizator"] != DBNull.Value)
                    {
                        textBox18.Text = (string)myReader["Organizator"];
                    }
                }
                myReader.Close();
                textBox5.Text = turnus;
                textBox7.Text = zlecenie;
            }
            catch (Exception blad5)
            {
                MessageBox.Show(blad5.ToString());
            }

            dataStreem2 = "";
            fill = "SELECT Od, Do FROM TURNUSY WHERE Turnus = '" + turnus + "'";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["Od"];      // osoba kontaktowa
                    dataStreem2 = (string)myReader["Do"];
                    textBox6.Text = dataStreem.Trim() + " - " + dataStreem2.Trim();
                }
                myReader.Close();
            }
            catch (Exception blad6)
            {
                MessageBox.Show(blad6.ToString());
            }


            //////////////////////////////////   Ustawia comboBox3 - wychowawcy
        }

        //zapisuje zmiany zleceniodawcy i wychowawcy o ile były zmienione
        private void ZapiszZmiany()
        {
            //zmienne wspolne

            SqlCommand myCommand;
            string fill;
            int powiadomiono = 0;
            bool err = false;

            fill = @"UPDATE GLOWNA SET 
            Zleceniodawca ='" + textBox7.Text + @"',
            Kod_pocz ='" + textBox11.Text + @"',
            Miejscowosc ='" + textBox12.Text + @"',
            Ul ='" + textBox4.Text + @"',
            Nr_lokalu ='" + textBox13.Text + @"',
            Os_kontaktowa ='" + textBox9.Text + @"',
            Tel_kontaktowy ='" + textBox10.Text + @"',
            Il_dzieci =" + Convert.ToInt16(textBox8.Text) + @",
            Powiadomiono =" + powiadomiono + @",
            Id_powiadom =" + 0 + @",
            Wyjazd ='" + textBox1.Text + @"',
            Powrot ='" + textBox3.Text + @"',
            Uwagi ='" + textBox2.Text + @"',
            Data_powiadom ='" + textBox17.Text + @"',
            Turnus ='" + textBox5.Text + @"',
            Organizator ='" + textBox18.Text + @"'
            WHERE 
                Turnus ='" + turnus + @"' 
            AND 
                Zleceniodawca ='" + zlecenie + @"' ";

                    try
                    {
                        //MessageBox.Show(fill);
                        myCommand = new SqlCommand(fill, myCon);
                        myCommand.ExecuteNonQuery();
                    }
                    catch (Exception blad2)
                    {
                        MessageBox.Show(blad2.ToString());
                        err = true;
                    }

            /*
                    string text = wybranyOperator.Substring(22, (wybranyOperator.Length - 22));
                    fill = "INSERT INTO ZMIANY (Kto, Id_zlec, Nazwa_zlec, Turnus, Zmiana) VALUES ('" + text + "', 0, '" + textBox7.Text + "', '" + textBox5.Text + "', '" + zmiany + "')";

                    try
                    {
                        //MessageBox.Show(fill);
                        myCommand = new SqlCommand(fill, myCon);
                        myCommand.ExecuteNonQuery();
                    }
                    catch (Exception blad1)
                    {
                        MessageBox.Show(blad1.ToString());
                        err = true;
                    }
            */
                    if (err) MessageBox.Show("Błąd podczas uaktualniania danych");
                    else
                    {
                        MessageBox.Show("Zapisano zmiany");
                    }

            zmieniono = false;
            for (int i = 0; i < 12; i++) zmiany_bool[i] = false;
            button4.Enabled = false;      
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ZapiszZmiany();
        }


        //drukowanie strony
        private void printDoc_PrintPage(Object sender, PrintPageEventArgs e)
        {
            string textToPrint =
                "\n \n \n"
                + textBox5.Text + "     data: " + textBox6.Text + "\n" +
                "Zleceniodawca: " + textBox7.Text + "\t" + "Ilość dzieci: " + textBox8.Text + "\n" +
                "\n Wyjazd \n" + textBox1.Text + "\n\n Powrót \n" + textBox3.Text + "\n\n" +
                "UWAGI :\n" + textBox2.Text + "\n";

            Font printFont = new Font("Times New Roman", 10);
            e.Graphics.DrawString(textToPrint, printFont, Brushes.Black, 30, 30);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintDialog dlg = new PrintDialog();
            dlg.UseEXDialog = true;
            dlg.Document = printDoc;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem;
            string fill;
            int count_turnus = 0;
            int count_all = 0;


            // zlicza ilosc dzieci na turnusie
            if ((textBox5.Text != "") && (textBox7.Text != ""))
            {
                fill = "";
                if (textBox5.Text != "") fill = "SELECT Il_dzieci FROM GLOWNA WHERE Turnus ='" + textBox5.Text + "'";
                dataStreem = "Il_dzieci";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        count_turnus += (int)myReader[dataStreem];
                    }
                    myReader.Close();
                }
                catch (Exception blad6)
                {
                    MessageBox.Show(blad6.ToString());
                }
            }
            // zlicza ilosc dzieci na wszystkich turnusach

            fill = "SELECT Il_dzieci FROM GLOWNA ";
            dataStreem = "Il_dzieci";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    count_all += (int)myReader[dataStreem];
                }
                myReader.Close();
            }
            catch (Exception blad7)
            {
                MessageBox.Show(blad7.ToString());
            }
            dataStreem = "Ilość dzieci na turnusie :" + textBox5.Text + "   === " + count_turnus.ToString() + "\n";
            fill = "Ilość dzieci na całych koloniach/feriach === " + count_all.ToString();
            dataStreem = (dataStreem + fill);
            MessageBox.Show(dataStreem);

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[0])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono nazwę zleceniodawcy \n";
                zmiany_bool[0] = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[1])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono il. uczestników \n";
                zmiany_bool[1] = true;
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[2])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono osobę kontaktową \n";
                zmiany_bool[2] = true;
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[3])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono tel. kontaktowy \n";
                zmiany_bool[3] = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[4])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono adres zlec. - ulica \n";
                zmiany_bool[4] = true;
            }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[5])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono turnus \n";
                zmiany_bool[5] = true;
            }
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[6])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono adres zlec. - poczta \n";
                zmiany_bool[6] = true;
            }
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[7])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono adres zlec. - nr. lok. \n";
                zmiany_bool[7] = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[8])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono wyjazd grupy \n";
                zmiany_bool[8] = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[9])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono powrót grupy \n";
                zmiany_bool[9] = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[10])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono - uwagi \n";
                zmiany_bool[10] = true;
            }
        }

        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[11])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono - Organizator \n";
                zmiany_bool[11] = true;
            }
        }
    }
}
