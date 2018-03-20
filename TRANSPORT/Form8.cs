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
    public partial class Form8 : Form
    {

        public Form9 dlg;

        class TreeWychowawcy
        {

            public List<string> zleceniodawca = new List<string>();
            public List<int> zlecTurnus = new List<int>();
            public List<string> turnus = new List<string>();
            public string packedTurnus;
            public string packedZleceniodawca;
            private int id;
            private string kto;
            private string telefon;
            private string kodPocz;
            private string ul;
            private string nrLok;
            private string poczta;
            private string uwagi;
            private string imie;
            private string nazwisko;
            private string dataUr;
            private string pesel;
            private string miasto;
            private string terminSpot;
            private string poBilety;
            private bool powiadom;

            // Default constructor:
            public TreeWychowawcy(
                List<int> zlecTur,
                List<char[]> tur,
                List<char[]> zle,
                string Imie,
                string Nazwisko,
                string DataUr,
                string Pesel,
                string Tel,
                string Ul,
                string NrLok,
                string Miasto,
                string KodPocz,
                string Poczta,
                string Uwagi,
                string TerminSpot,
                string PoBilety,
                bool Powiadom,
                int idWych)
            {


                this.zlecTurnus.Clear();
                foreach (int t in zlecTur)
                {
                    zlecTurnus.Add(t);
                }

                this.turnus.Clear();
                foreach (char[] tmp in tur)
                {
                    string data = new string(tmp);
                    this.turnus.Add(data);
                }

                this.zleceniodawca.Clear();
                foreach (char[] tmp2 in zle)
                {
                    string data = new string(tmp2);
                    this.zleceniodawca.Add(data);
                }

                this.imie = Imie;
                this.nazwisko = Nazwisko;
                this.dataUr = DataUr;
                this.pesel = Pesel;
                this.telefon = Tel;
                this.ul = Ul;
                this.nrLok = NrLok;
                this.miasto = Miasto;
                this.kodPocz = KodPocz;
                this.poczta = Poczta;
                this.uwagi = Uwagi;
                this.terminSpot = TerminSpot;
                this.poBilety = PoBilety;
                this.powiadom = Powiadom;
                this.id = idWych;
                this.kto = "";
            }
            public TreeWychowawcy(
                List<int> zlecTur,
                List<char[]> tur,
                List<char[]> zle,
                string Imie,
                string Nazwisko,
                string DataUr,
                string Pesel,
                string Tel,
                string Ul,
                string NrLok,
                string Miasto,
                string KodPocz,
                string Poczta,
                string Uwagi,
                string TerminSpot,
                string PoBilety,
                bool Powiadom,
                int idWych,
                string orgTurnus,
                string orgZleceniodawca)
            {


                this.zlecTurnus.Clear();
                foreach (int t in zlecTur)
                {
                    zlecTurnus.Add(t);
                }

                this.turnus.Clear();
                foreach (char[] tmp in tur)
                {
                    string data = new string(tmp);
                    this.turnus.Add(data);
                }

                this.zleceniodawca.Clear();
                foreach (char[] tmp2 in zle)
                {
                    string data = new string(tmp2);
                    this.zleceniodawca.Add(data);
                }

                this.imie = Imie;
                this.nazwisko = Nazwisko;
                this.dataUr = DataUr;
                this.pesel = Pesel;
                this.telefon = Tel;
                this.ul = Ul;
                this.nrLok = NrLok;
                this.miasto = Miasto;
                this.kodPocz = KodPocz;
                this.poczta = Poczta;
                this.uwagi = Uwagi;
                this.terminSpot = TerminSpot;
                this.poBilety = PoBilety;
                this.powiadom = Powiadom;
                this.id = idWych;
                this.packedTurnus = orgTurnus;
                this.packedZleceniodawca = orgZleceniodawca;
            }


            public string Telefon()
            {
                return this.telefon;
            }
            public string Imie()
            {
                return this.imie;
            }
            public string Nazwisko()
            {
                return this.nazwisko;
            }
            public string Ul()
            {
                return this.ul;
            }
            public string NrLok()
            {
                return this.nrLok;
            }
            public string Poczta()
            {
                return this.poczta;
            }
            public string Uwagi()
            {
                return this.uwagi;
            }
            public int Id()
            {
                return this.id;
            }
            public string DataUr()
            {
                return this.dataUr;
            }
            public string Pesel()
            {
                return this.pesel;
            }
            public string Miasto()
            {
                return this.miasto;
            }
            public string KodPocz()
            {
                return this.kodPocz;
            }
            public string TerminSpot()
            {
                return this.terminSpot;
            }
            public string PoBilety()
            {
                return this.poBilety;
            }
            public bool Powiadom()
            {
                return this.powiadom;
            }
            public bool containsTurnus(string s)
            {
                return this.turnus.Contains(s);
            }
            public bool containsZleceniodawce(string s)
            {
                return this.zleceniodawca.Contains(s);
            }
            public bool containsKto(string s)
            {
                if (s == this.kto) return true;
                else return false;
            }
            public int CountZlec()
            {
                return this.zleceniodawca.Count;
            }
            public string GetZlec(int i)
            {
                if ((i < this.zleceniodawca.Count) && (i >= 0)) return this.zleceniodawca[i];
                else return "";
            }
            public int GetIdTur(int i)
            {
                if ((i < this.zleceniodawca.Count) && (i >= 0)) return this.zlecTurnus[i];
                else return -1;
            }

        }


        List<Turnusy> listTurnusy;
        private SqlConnection myCon;
        private List<TreeWychowawcy> treeWychowawcy; //zmienna pomocnicza dla zakładki wychowawcy - zawiera kompletne dane aktualnie wyświetlanych wychowawców w listview2
        private int wybranyWych = -1;

        public Form8(SqlConnection con, int tmp)
        {
            myCon = con;
            wybranyWych = tmp;
            InitializeComponent();
        }

        private void Form8_Load(object sender, EventArgs e)
        {


            if (myCon.State != ConnectionState.Open)
            {
                    MessageBox.Show("ERROR\nConnection is closed\n");
                    Close();
            }

            FillUp();
        }

        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private List<char[]> Unpack(string data)
        {
            int count = 0;
            int cnt = 0;

            List<char[]> temp = new List<char[]>();
            List<char> tmp = new List<char>();
            if (data.Length > 0)
            {
                count = 0;
                while (count < data.Length)
                {
                    if (data[count] != ';')
                    {
                        tmp.Add(data[count]);
                    }
                    else
                    {
                        cnt = tmp.Count;
                        char[] turnus = new char[cnt];
                        for (int i = 0; i < cnt; i++)
                        {
                            turnus[i] = tmp[i];
                        }

                        temp.Add(turnus);
                        tmp.Clear();
                    }

                    count++;

                    if (count >= data.Length)
                    {

                        cnt = tmp.Count;
                        char[] unp = new char[cnt];
                        for (int i = 0; i < cnt; i++)
                        {
                            unp[i] = tmp[i];
                        }

                        temp.Add(unp);
                        tmp.Clear();

                    }
                }
            }
            else
            {
                char[] unp = { 'E', 'm', 'p', 't', 'y' };
                temp.Add(unp);
            }
            /*
                        // set =0 - rozdziela sie turnusy set =1 - rozdziela sie zleceniodawcow i trzeba jeszcze rozdzielic je na pary zleceniodawca-turnus
                        if (set == 0) return temp;
                        else
                            if (set == 1)
                            {
                                return temp;
                            }
                            else
                            {
                                MessageBox.Show("wewnętrzny błąd funkcji unpack ");
                                Close();
                            }
             */
            return temp;

        }
        private List<int> UnpackZlec(List<char[]> data)
        {
            List<int> temp = new List<int>();

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Length > 2)
                {
                    if (data[i][data[i].Length - 2] == ':')
                    {
                        string str = data[i][data[i].Length - 1].ToString();
                        temp.Add(Convert.ToInt32(str));
                        //MessageBox.Show( temp[i].ToString() );
                    }
                    else
                    {
                        temp.Add(-1);
                    }
                }
                else
                {
                    temp.Add(-1);
                }
            }

            //MessageBox.Show( "Ilosc elementow : " + temp.Count.ToString() );
            return temp;

        }


        // wyświetla wszystkie dane zleceniodawcy + combobox wychowawcy + combobox turnusy
        private void FillUp()
        {

            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;

            bool powiadom = false;
            int idWych = -1;
            string[] data = new string[15];
            for (int ii = 0; ii < 15; ii++)
            {
                data[ii] = "";
            }

            List<char[]> tempZlec = new List<char[]>(); //- tymczasowa tabela zleceniodawcow do ktorych przipisany jest wychowawca
            List<char[]> tempZlec2 = new List<char[]>(); //- tymczasowa tabela zleceniodawcow ( usunieto ostatnie symbole odnosnika do turnusow )do ktorych przipisany jest wychowawca

            List<char[]> tempTurnus = new List<char[]>(); // tymczasowa tabela turnusow na ktore zapisany jest wychowawca
            List<int> tempzlecTurnus = new List<int>(); // tymczasowa tabela okreslajaca do ktorego turnusa nalezy dany zleceniodawca (para index zleceniodawcy i taki sam index tego = wartosc pola id turnusa na ktorym jest dany zlecedeniodawca)

            if (treeWychowawcy == null)
            {
                treeWychowawcy = new List<TreeWychowawcy>();
            }
            else
            {
                treeWychowawcy.Clear();
            }

            TreeWychowawcy tempWych;
            char[] unp = { 'E', 'm', 'p', 't', 'y' };
            if (wybranyWych >= 0)
            {
                fill = "SELECT Id, Turnus, Zleceniodawca, Imie, Nazwisko, DataUr, Pesel, Telefon, Ul, Nr_lokalu,Miasto, Kod_pocz, Poczta, Uwagi, TerminSpot, PoBilety, Powiadom FROM WYCHOWAWCY WHERE Id = " + wybranyWych;
                //MessageBox.Show("wybrany : " + wybranyWych);
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        idWych = (int)myReader["Id"];
                        if (myReader["Turnus"] != DBNull.Value) data[0] = (string)myReader["Turnus"];      // wychowawca
                        if (myReader["Zleceniodawca"] != DBNull.Value) data[1] = (string)myReader["Zleceniodawca"];
                        if (myReader["Imie"] != DBNull.Value) data[2] = (string)myReader["Imie"];
                        if (myReader["Nazwisko"] != DBNull.Value) data[3] = (string)myReader["Nazwisko"];
                        if (myReader["DataUr"] != DBNull.Value) data[4] = (string)myReader["DataUr"];      // wychowawca
                        if (myReader["Pesel"] != DBNull.Value) data[5] = (string)myReader["Pesel"];
                        if (myReader["Telefon"] != DBNull.Value) data[6] = (string)myReader["Telefon"];
                        if (myReader["Ul"] != DBNull.Value) data[7] = (string)myReader["Ul"];
                        if (myReader["Nr_lokalu"] != DBNull.Value) data[8] = (string)myReader["Nr_lokalu"];      // wychowawca
                        if (myReader["Miasto"] != DBNull.Value) data[9] = (string)myReader["Miasto"];
                        if (myReader["Kod_pocz"] != DBNull.Value) data[10] = (string)myReader["Kod_pocz"];
                        if (myReader["Poczta"] != DBNull.Value) data[11] = (string)myReader["Poczta"];
                        if (myReader["Uwagi"] != DBNull.Value) data[12] = (string)myReader["Uwagi"];
                        if (myReader["TerminSpot"] != DBNull.Value) data[13] = (string)myReader["TerminSpot"];      // wychowawca
                        if (myReader["PoBilety"] != DBNull.Value) data[14] = (string)myReader["PoBilety"];
                        if (myReader["Powiadom"] != DBNull.Value) powiadom = (bool)myReader["Powiadom"];

                        tempTurnus.Clear();
                        tempTurnus = Unpack(data[0]);

                        tempZlec.Clear();
                        tempZlec = Unpack(data[1]);
                        tempzlecTurnus = UnpackZlec(tempZlec);

                        tempZlec2.Clear();
                        // usuwa znaczniki przynaleznosci do danego turnusa
                        foreach (char[] tmpStr in tempZlec)
                        {
                            if (tmpStr[tmpStr.Length - 2] == ':')
                            {
                                char[] tmpStr2 = new char[tmpStr.Length - 2];
                                for (int ii = 0; ii < (tmpStr.Length - 2); ii++)
                                {
                                    tmpStr2[ii] = tmpStr[ii];
                                }
                                tempZlec2.Add(tmpStr2);
                            }
                            else
                                tempZlec2.Add(tmpStr);
                        }

                        // upewnia sie ze nie bedzie nic = null
                        //for (int i = 0; i < 15; i++)
                        //{
                        //    if (data[i] == "") data[i] = "-";
                        //}
                        //data[8] = "lolek";
                        if (tempTurnus.Count == 0) tempTurnus.Add(unp);
                        if (tempZlec2.Count == 0) tempZlec2.Add(unp);
                        if (tempzlecTurnus.Count == 0) tempzlecTurnus.Add(-1);

                        tempWych = new TreeWychowawcy(tempzlecTurnus, tempTurnus, tempZlec2, data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11], data[12], data[13], data[14], powiadom, idWych);
                        idWych = -1;
                        treeWychowawcy.Add(tempWych);

                    }
                    myReader.Close();

                    if (treeWychowawcy.Count > 0)
                    {
                        textBox1.Text = treeWychowawcy[0].Imie();
                        textBox9.Text = treeWychowawcy[0].Nazwisko();
                        textBox8.Text = treeWychowawcy[0].DataUr();
                        textBox12.Text = treeWychowawcy[0].Pesel();
                        textBox2.Text = treeWychowawcy[0].Telefon();
                        textBox3.Text = treeWychowawcy[0].Ul();
                        textBox13.Text = treeWychowawcy[0].NrLok();
                        textBox7.Text = treeWychowawcy[0].Miasto();
                        textBox4.Text = treeWychowawcy[0].KodPocz();
                        textBox5.Text = treeWychowawcy[0].Poczta();
                        textBox6.Text = treeWychowawcy[0].Uwagi();
                        textBox10.Text = treeWychowawcy[0].TerminSpot();
                        textBox11.Text = treeWychowawcy[0].PoBilety();
                        if(treeWychowawcy[0].Powiadom()) checkBox1.Checked = true;
                            else checkBox1.Checked = false;

                        
                        checkedListBox1.Items.Clear();
                        for (int i = 0; i < treeWychowawcy[0].turnus.Count; i++)
                        {
                            checkedListBox1.Items.Add( treeWychowawcy[0].turnus[i] );
                            checkedListBox1.SetItemChecked(i, true);
                        }

                        checkedListBox2.Items.Clear();
                        for (int i = 0; i < treeWychowawcy[0].zleceniodawca.Count; i++)
                        {
                            checkedListBox2.Items.Add(treeWychowawcy[0].zleceniodawca[i]);
                        }


                    }
                }
                catch (Exception blad7)
                {
                    MessageBox.Show(blad7.ToString());
                }
            }//if(wybranyWych != "*")


            //label1.Text = "Imię";
            //label9.Text = "Nazwisko";
            //label2.Text = "Telefon";
            //label3.Text = "Ulica";
            //label4.Text = "Kod pocz.";
            //label5.Text = "Poczta";
            //label6.Text = "Uwagi";
            //label7.Text = "Wpisany na zlecenia:";
            //label8.Text = "Wpisany na turnusy:";

            button1.Text = "Dodaj";
            button2.Text = "Anuluj";
            //button3.Text = "Dodaj turnus";
            button1.Focus();
        }
        //Anuluj
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        //dodaj
        private void button1_Click(object sender, EventArgs e)
        {

            SqlCommand myCommand;
            string fill = "";
            string turnusy = "";
            string zlecenia = "";

            if(wybranyWych == -1)
            {
                try
                {
                    fill = @"INSERT INTO WYCHOWAWCY(
                            Turnus,
                            Zleceniodawca,
                            Imie,
                            Nazwisko,
                            DataUr,
                            Pesel,
                            Telefon,
                            Ul,
                            Nr_lokalu,
                            Miasto,
                            Kod_pocz,
                            Poczta,
                            Uwagi,
                            TerminSpot,
                            PoBilety
                            Powiadom )
                        VALUES (
                               '" +  turnusy +
                               "','" + zlecenia +
                               "','" + textBox1.Text +
                               "','" + textBox9.Text +
                               "','" + textBox8.Text +
                               "','" + textBox12.Text +
                               "','" + textBox2.Text +
                               "','" + textBox3.Text +
                               "','" + textBox13.Text +
                               "','" + textBox7.Text +
                               "','" + textBox4.Text +
                               "','" + textBox5.Text +
                               "','" + textBox6.Text +
                               "','" + textBox10.Text +
                               "','" + textBox11.Text +
                               "',0)";
                    myCommand = new SqlCommand(fill, myCon);
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
            else
            {
                fill = @"UPDATE WYCHOWAWCY SET 
                                Turnus ='" + turnusy + @"',
                                Zleceniodawca ='" + zlecenia + @"',
                                Imie ='" + textBox1.Text + @"',
                                Nazwisko ='" + textBox9.Text + @"',
                                DataUr ='" + textBox8.Text + @"',
                                Pesel ='" + textBox12.Text + @"',
                                Telefon ='" + textBox2.Text + @"',
                                Ul =" + textBox3.Text + @",
                                Nr_lokalu =" + textBox13.Text + @",
                                Miasto =" + textBox7.Text + @",
                                Kod_pocz ='" + textBox4.Text + @"',
                                Poczta ='" + textBox5.Text + @"',
                                Uwagi ='" + textBox6.Text + @"',
                                TerminSpot ='" + textBox10.Text + @"',
                                PoBilety ='" + textBox11.Text + @"',
                                Powiadom = " + 0 + " " + @"
                            WHERE 
                                Id = " + wybranyWych;
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad1)
                {
                    MessageBox.Show(blad1.ToString());
                }
            }
        }



        private void FillUpTurnusy()
        {

            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string[] dataStreem = new string[6];
            int il_Dz = 0;
            int il_Wych = 0;
            Turnusy tempTurnus;

            if (listTurnusy == null)
            {
                listTurnusy = new List<Turnusy>();
            }
            else
                listTurnusy.Clear();


            fill = "SELECT Turnus, Od, Do, il_wych, il_dzieci, Kierownik FROM TURNUSY";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem[1] = (string)myReader["Od"];
                    dataStreem[2] = (string)myReader["Do"];
                    dataStreem[0] = (string)myReader["Turnus"];
                    il_Wych = (int)myReader["il_wych"];
                    il_Dz = (int)myReader["il_dzieci"];
                    dataStreem[5] = (string)myReader["Kierownik"];

                    tempTurnus = new Turnusy(dataStreem[0], dataStreem[1], dataStreem[2], dataStreem[5], il_Dz, il_Wych);
                    listTurnusy.Add(tempTurnus);

                }
                myReader.Close();
            }
            catch (Exception blad4)
            {
                MessageBox.Show(blad4.ToString());
            }

        }

    }
}
