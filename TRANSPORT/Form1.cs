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
using System.Drawing.Printing; // do drukowania
using System.Collections;  //->> ArrayList dynamiczna tablica

using System.Threading; // Required for this example


namespace TRANSPORT
{


    public partial class Form1 : Form
    {


        /// <summary>
        /// Dodatkowe klasy używana w programie
        /// </summary>
        class TreeWychowawcy
        {
            public List <string> zleceniodawca = new List<string>();
            public List<int> zlecTurnus = new List<int>();
            public List <string> turnus = new List<string>();
            public string packedTurnus;
            public string packedZleceniodawca;
            private int id;
            private string kto;
            private string telefon;
            private string kod_pocz;
            private string ul;
            private string nr_lokalu;
            private string poczta;
            private string uwagi;
            // Default constructor:
            public TreeWychowawcy( List<int> zlecTur, List<char[]> tur, List<char[]> zle, string kt, string tel, string kod, string ulica, string nr_lok, string pocz, string uwagi, int idWych  )
            {


                this.zlecTurnus.Clear();
                foreach (int t in zlecTur)
                {
                    zlecTurnus.Add(t);
                }

                this.turnus.Clear();
                foreach( char[] tmp in tur)
                {
                    string data = new string(tmp);
                    this.turnus.Add( data );
                }
                
                this.zleceniodawca.Clear();
                foreach (char[] tmp2 in zle )
                {
                    string data = new string(tmp2);
                    this.zleceniodawca.Add( data );
                }
                
                this.kto = kt;
                this.ul = ulica;
                this.telefon = tel;
                this.kod_pocz = kod;
                this.nr_lokalu = nr_lok;
                this.poczta = pocz;
                this.uwagi = uwagi;
                this.id = idWych;

                
            }
            public TreeWychowawcy(List<int> zlecTur, List<char[]> tur, List<char[]> zle, string kt, string tel, string kod, string ulica, string nr_lok, string pocz, string uwagi, int idWych, string orgTurnus, string orgZleceniodawca)
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

                this.kto = kt;
                this.ul = ulica;
                this.telefon = tel;
                this.kod_pocz = kod;
                this.nr_lokalu = nr_lok;
                this.poczta = pocz;
                this.uwagi = uwagi;
                this.id = idWych;
                this.packedTurnus = orgTurnus;
                this.packedZleceniodawca = orgZleceniodawca;


            }


            public string Telefon()
            {
                return this.telefon;
            }
            public string Kto()
            {
                return this.kto;
            }
            public string Kod_pocz()
            {
                return this.kod_pocz;
            }
            public string Ul()
            {
                return this.ul;
            }
            public string Nr_lokalu()
            {
                return this.nr_lokalu;
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
        class Tab1Leave
        {
            public string turnus;
            public string zleceniodawca;
            //public int wybTurnus;
            //public int wybZleceniodawca;
            
            public Tab1Leave()
            {
                turnus = "";
                zleceniodawca = "";
            }

            public Tab1Leave(string tur, string zlec)
            {
                turnus = tur;
                zleceniodawca = zlec;
            }

        }

        // Klasy odpowiedzialne za sortowanie w tab - > zlecenia listview7
        /////////////////////////////////////////////////////
        // An instance of the SortWrapper class is created for
        // each item and added to the ArrayList for sorting.
        public class SortWrapper
        {
            internal ListViewItem sortItem;
            internal int sortColumn;


            // A SortWrapper requires the item and the index of the clicked column.
            public SortWrapper(ListViewItem Item, int iColumn)
            {
                sortItem = Item;
                sortColumn = iColumn;
            }

            // Text property for getting the text of an item.
            public string Text
            {
                get
                {
                    return sortItem.SubItems[sortColumn].Text;
                }
            }

            // Implementation of the IComparer
            // interface for sorting ArrayList items.
            public class SortComparer : IComparer
            {
                bool ascending;

                // Constructor requires the sort order;
                // true if ascending, otherwise descending.
                public SortComparer(bool asc)
                {
                    this.ascending = asc;
                }

                // Implemnentation of the IComparer:Compare
                // method for comparing two objects.
                public int Compare(object x, object y)
                {
                    //if(  )
                    SortWrapper xItem = (SortWrapper)x;
                    SortWrapper yItem = (SortWrapper)y;

                    string xText = xItem.sortItem.SubItems[xItem.sortColumn].Text;
                    string yText = yItem.sortItem.SubItems[yItem.sortColumn].Text;
                    if ((xItem.sortColumn == 4) || (xItem.sortColumn == 0) || (xItem.sortColumn == 1))
                    {
                        int xx = Convert.ToInt16(xItem.sortItem.SubItems[xItem.sortColumn].Text);
                        int yy = Convert.ToInt16(yItem.sortItem.SubItems[yItem.sortColumn].Text);
                        if (this.ascending)
                        {
                            if ((xx - yy) > 0) return 1;
                            else
                            {
                                if ((xx - yy) == 0) return 0;
                                else return -1;
                            }
                        }
                        else
                        {
                            if ((xx - yy) > 0) return -1;
                            else
                            {
                                if ((xx - yy) == 0) return 0;
                                else return 1;
                            }
                        }
                    }
                    else
                    {
                        return xText.CompareTo(yText) * (this.ascending ? 1 : -1);
                    }
                }
            }
        }
        // The ColHeader class is a ColumnHeader object with an
        // added property for determining an ascending or descending sort.
        // True specifies an ascending order, false specifies a descending order.
        public class ColHeader : ColumnHeader
        {
            public bool ascending;
            public ColHeader(string text, int width, HorizontalAlignment align, bool asc)
            {
                this.Text = text;
                this.Width = width;
                this.TextAlign = align;
                this.ascending = asc;
            }
        }
        ////////////////////////////////////////////////////


        //  Zmienne dostepne w calym programie
        char tab = '\u0009';
        private PrintDocument printDoc; // zmienna do drukowania
        private SqlConnection myCon; // zmienna do połączenia SQL
        private bool zmieniono = false; // czy zmieniono cos w danych zleceniodawcy (zakładka główna)jeżeli tak to staje się aktywny przycisk zapisywania
        private bool zmienionoWychowawce = false; //czy zmieniono coś w danych wychowawcy na zakładce główna - w sumie tylko telefon lub imię/nazwisko
        private bool zmienionoTab2 = false; //czy zmieniono cos na zakładce turnusy
        private int combo2SelectedIndex = -1; //jak sie wybierze anuluj to ustawia tutaj aktualna wartosc comboBox2.SelectedItem dzieki czemu nie pozwala na zmiane na inna wartosc
        private int combo1SelectedIndex = -1;
        private ArrayList tab2ListBox;
        private List<string> tab3CheckedListBox1; //pomocnicza znienna zawierająca nazwy turnusów wyświetlanych w combobox4 na zakładce wychowawcy
        private List <TreeWychowawcy> treeWychowawcy; //zmienna pomocnicza dla zakładki wychowawcy - zawiera kompletne dane aktualnie wyświetlanych wychowawców w listview2
        private int tabWychListWychFocus = -1; // zapisuje wartosc wybranego indexu wychowawcy kiedy opuszcza zakladke wychowawcy
        private Tab1Leave tab1Leave; // zapisuje podstawowe dane przy wychodzeniu z zakladki aby ponownie je ustawic kiedy sie na nia przejdzie
        private string zmiany = ""; // zapisuje które rekordy zostały zmienione -> a pozniej zapisuje to do tabeli "Zmiany" zeby kazdy wiedzial co zostalo zienione
        private bool[] zmiany_bool = new bool[12];
        private bool lastTick = false;
        private string curentBase = "";
        private List<int> wybTurnusy = new List<int>(); // pomocnicza zawiera liste wybranych turnusow w checListkBox1
        private int currentWybTur = 0;// pomocnicza dla zakladki Wychowawcy

        private ColHeader saved_clickedCol;
        private bool saved_ascending;
        private int saved_column_id;
        private bool isSavedListView7 = false;
        private int saveSelectedItem = 0;

        public Form3 form3;
        // koniec

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 11; i++)
            {
                zmiany_bool[i] = false;
            }

            printDoc = new PrintDocument(); //ustawia zmienna globalna do drukowania
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage); //wiąze kazde wywołanie print z funkcją printDoc_PrintPage

            tab1Leave = new Tab1Leave();
            tab1Leave.turnus = "";
            tab1Leave.zleceniodawca = "";
        }

        //pierwsze czynności przy uruchamianiu się programu -> ładuje się zakładka główna
        private void Form1_Load(object sender, EventArgs e)
        {
            //timer1.Enabled = true;
            //timer1.Start();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            string id = null;
            string pw = null;
            string server = null;
            string trusted = null;
            string database = null;
            string timeout = null;

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
                id = read.ReadLine();
                id = Program.XorString(id, Program.XOR_KEY);
                pw = read.ReadLine();
                pw = Program.XorString(pw, Program.XOR_KEY);
                server = read.ReadLine();
                server = Program.XorString(server, Program.XOR_KEY);
                trusted = read.ReadLine();
                trusted = Program.XorString(trusted, Program.XOR_KEY);
                database = read.ReadLine();
                database = Program.XorString(database, Program.XOR_KEY);
                timeout = read.ReadLine();
                timeout = Program.XorString(timeout, Program.XOR_KEY);
                read.Close();
                if ((id == null) || (pw == null) || (server == null) || (trusted == null) || (database == null) || (timeout == null))
                {
                    MessageBox.Show("Błąd w pliku system.ini\n");
                    Close();
                    return;
                }
                else
                {
                    try
                    {
                        myCon = new SqlConnection(id + pw + server + trusted + database + timeout);
                    }
                    catch(Exception er)
                    {
                        MessageBox.Show("Error: " + er.ToString());
                        Close();
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Nie udało się otowrzyć pliku system.ini \n Błąd połączenia z bazą SQL \n");
                //myCon = new SqlConnection("server=(local)\\SQLEXPRESS;database=TRANSPORT_2009;Integrated Security=SSPI");
                //myCon = new SqlConnection("server=IREK\\CDN_OPTIMA;database=TRANSPORT_2009;Integrated Security=SSPI");
                //MessageBox.Show("nie udało sie odnaleźć pliku system.ini \n Domyślne połączenie z SQL to : \n server=(local)\\SQLEXPRESS_KAROL;database=main;Integrated Security=SSPI");
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
                    return;
                }
            }
            else
            {
                MessageBox.Show("Nie udało się nawiązać połaczenia z SQL");
                Close();
                return;
            }

            this.form3 = new Form3(myCon);
            this.form3.ValueUpdated += new ValueUpdatedEventHandler(dlg_ValueUpdated);
            this.form3.ShowDialog();
            
            if (myCon.State == ConnectionState.Open)
            {
                myCon.Close();
            }

            //MessageBox.Show("Wybrano BASE : " + curentBase + "\n");

            if (curentBase != null) myCon = new SqlConnection(id + pw + server + trusted + "database=" + curentBase + ";" + timeout);

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
                    return;
                }
            }
            else
            {
                MessageBox.Show("Nie udało się nawiązać połaczenia z SQL");
                Close();
                return;
            }
            label25.Text = label26.Text = label27.Text = label28.Text = label31.Text = label48.Text = label49.Text = label50.Text = label51.Text = curentBase;
            
            tabPage8_Enter(sender, e);

        }


        private void dlg_ValueUpdated(object sender, ValueUpdatedEventArgs e)
        {
            if (e.NewValue.val == 3)
            {
                if (e.NewValue.stringVal == "close")
                {
                    Close();
                }
                else
                {
                    toolStripStatusLabel1.Text = "Zalogowany operator : " + e.NewValue.stringVal;
                }
            }
            else
                if (e.NewValue.val == 4)
                {
                    if (e.NewValue.stringVal == "close")
                    {
                        Close();
                    }
                    else
                    {
                         curentBase = e.NewValue.stringVal;
                    }
                }
        }

        //sprawdza czy podany wychowawca istnieje w bazie
        private bool IstniejeWychowawca(string data)
        {
            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem = "";
            string fill;
            int count = 0;

            fill = "SELECT Kto FROM WYCHOWAWCY WHERE Kto ='" + data + "'";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["Kto"];
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
        //sprawdza czy podany zleceniodawca istnieje w bazie 'glowna'
        private bool IstniejeZleceniodawca(string strZlec, string strTurnus)
        {
            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem = "";
            string fill;
            int count = 0;

            fill = "SELECT Zleceniodawca FROM GLOWNA WHERE Zleceniodawca ='" + strZlec + "' AND Turnus ='" + strTurnus + "'";
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
        //Zapisuje nowego wychowawce lub nadpisuje istniejącego
        private void SaveWych(TreeWychowawcy wych)
        {
            SqlCommand myCommand;
            string fill;

            if (wych.Id() == -1)
            {
                fill = @"INSERT INTO WYCHOWAWCY (Turnus,Zleceniodawca,Kto,Telefon,Kod_pocz,Poczta,Ul,Nr_lokalu) VALUES (
            '" + wych.packedTurnus + @"',
            '" + wych.packedZleceniodawca + @"',
            '" + wych.Kto() + @"',
            '" + wych.Telefon() + @"',
            '" + wych.Kod_pocz() + @"',
            '" + wych.Poczta() + @"',
            '" + wych.Ul() + @"',
            '" + wych.Nr_lokalu() + @"')";
            
            }
            else
                if (wych.Id() > -1)
                {
                    fill = @"UPDATE WYCHOWAWCY SET 
                    Turnus ='" + wych.packedTurnus + @"',
                    Zleceniodawca ='" + wych.packedZleceniodawca + @"',
                    Kto ='" + wych.Kto() + @"',
                    Telefon ='" + wych.Telefon() + @"',
                    Kod_pocz ='" + wych.Kod_pocz() + @"',
                    Poczta ='" + wych.Poczta() + @"',
                    Ul ='" + wych.Ul() + @"',
                    Nr_lokalu ='" + wych.Nr_lokalu() + @"'
                    WHERE Id =" + wych.Id();
                }
                else
                {
                    MessageBox.Show("Błąd zapisu wychowawcy (niepoprawne id)");
                    return;
                }

            // zapis
            try
               {
                 //MessageBox.Show(fill);
                 myCommand = new SqlCommand(fill, myCon);
                 myCommand.ExecuteNonQuery();
               }
            catch (Exception blad1)
               {
                 MessageBox.Show(blad1.ToString());
               }
        }//end of function
        //spakowuje w jeden sformatowany string dane
        private string PackTurnus(List<string> turnus)
        {
            string packedTurnus = "";
            for(int i = 0; i < turnus.Count; i++)
            {
                packedTurnus += turnus[i];
                if( (i + 1) < turnus.Count ) packedTurnus += ";";
            }
            return packedTurnus;
        }
        //spakowuje w jeden sformatowany string dane
        private string PackZleceniodawca(List<string> zleceniodawca, List<int> idTurnusZlec)
        {
            string packedZlece = "";
            for (int i = 0; i < zleceniodawca.Count; i++)
            {
                packedZlece += zleceniodawca[i];
                packedZlece += ":";
                packedZlece += idTurnusZlec[i];

            }
            return packedZlece;
        }
        //sprawdza i usuwa jezeli podany zleceniodawca istnieje u danego wychowawcy - baza 'wychowawcy'
        private void ClearWych(string strZlec, string strTurnus)
        {
            SqlDataReader myReader;
            SqlCommand myCommand;
            List<TreeWychowawcy> wychowawcy = new List<TreeWychowawcy>();

            string fill;
            int idTurnus = -1;
            int idTempWych = -1;
            string[] data = new string[9];
            List<int> idWych = new List<int>();
            List<char[]> tempZlec = new List<char[]>(); //- tymczasowa tabela zleceniodawcow do ktorych przipisany jest wychowawca
            List<char[]> tempZlec2 = new List<char[]>(); //- tymczasowa tabela zleceniodawcow ( usunieto ostatnie symbole odnosnika do turnusow )do ktorych przipisany jest wychowawca

            List<char[]> tempTurnus = new List<char[]>(); // tymczasowa tabela turnusow na ktore zapisany jest wychowawca
            List<int> tempzlecTurnus = new List<int>(); // tymczasowa tabela okreslajaca do ktorego turnusa nalezy dany zleceniodawca (para index zleceniodawcy i taki sam index tego = wartosc pola id turnusa na ktorym jest dany zlecedeniodawca)


            TreeWychowawcy tempWych;
            char[] unp = { 'E', 'm', 'p', 't', 'y' };


            fill = "SELECT Id FROM TURNUSY WHERE Turnus ='" + strTurnus + "'";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    idTurnus = (int)myReader["Id"];
                }
                myReader.Close();
            }
            catch (Exception blad1)
            {
                MessageBox.Show(blad1.ToString());
            }


            if (idTurnus > -1)
            {

                fill = "SELECT Id, Turnus, Zleceniodawca FROM WYCHOWAWCY";

                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        idTempWych = (int)myReader["Id"];
                        data[0] = (string)myReader["Turnus"];      // wychowawca
                        data[1] = (string)myReader["Zleceniodawca"];

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
                        for (int i = 0; i < 9; i++)
                        {
                            if (data[i] == "") data[i] = "empty";
                        }

                        if (tempTurnus.Count == 0) tempTurnus.Add(unp);
                        if (tempZlec2.Count == 0) tempZlec2.Add(unp);
                        if (tempzlecTurnus.Count == 0) tempzlecTurnus.Add(-1);

                        tempWych = new TreeWychowawcy(tempzlecTurnus, tempTurnus, tempZlec2, data[2], data[3], data[4], data[5], data[6], data[7], data[8], idTempWych, data[0], data[1] );
                        idTempWych = -1;
                        wychowawcy.Add(tempWych);

                    }
                    myReader.Close();
                }
                catch (Exception blad7)
                {
                    MessageBox.Show(blad7.ToString());
                }

                //przeglada w petli i poprawia zleceniodawcow
                for (int i = 0; i < wychowawcy.Count; i++)
                {
                    if (wychowawcy[i].containsZleceniodawce(strZlec) )
                    {
                        for (int ii = 0; ii < wychowawcy[i].zleceniodawca.Count; ii++)
                        {
                            if( (wychowawcy[i].zleceniodawca[ii] == strZlec) && (wychowawcy[i].zlecTurnus[ii] == idTurnus) )
                            {
                                wychowawcy[i].zleceniodawca.RemoveAt(ii);
                                wychowawcy[i].zlecTurnus.RemoveAt(ii);
                                idWych.Add(i); // zapisuje index wych który uległ zmianie - zeby później zapisać te zmiany
                                wychowawcy[i].packedTurnus = PackTurnus(wychowawcy[i].turnus);
                                wychowawcy[i].packedZleceniodawca = PackZleceniodawca(wychowawcy[i].zleceniodawca, wychowawcy[i].zlecTurnus);

                            }
                        }
                    }
                }
            }//if(idTurnus > -1)

            for (int t = 0; t < idWych.Count; t++)
            {
                SaveWych(wychowawcy[idWych[t]]);
            }

        }//end of function

        //ustawia wartości początkowe zakładki główna = czyści i od nowa zgrywa dane zleżnie od argumentu
        private void CleanUp( int arg)
        {
            /////////////////////////////////////////////////////////////// Blok wczytywania poczatkowych danych
            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem;
            string fill;

            //Wczytywanie danych zleceniodawcow  dla comboBox2
            switch ( arg )
            {
                case 2 :
////////////////
                comboBox3.Items.Clear();
                comboBox2.Items.Clear();  //czysci na wszelki wypadek dane w boxie
                fill = "SELECT Zleceniodawca FROM GLOWNA";
                try
                {

                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();


                    while (myReader.Read())
                    {

                        dataStreem = (string)myReader["Zleceniodawca"];
                        comboBox2.Items.Add(dataStreem);
                    }
                    myReader.Close();
                }
                catch (Exception blad1)
                {
                    MessageBox.Show(blad1.ToString());
                }
                break;

///////////////
                case 1 :
                //Wczytywanie danych turnusow  dla comboBox1
                comboBox3.Items.Clear();
                comboBox1.Items.Clear();  //czysci na wszelki wypadek dane w boxie
                fill = "SELECT Turnus FROM TURNUSY";

                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        dataStreem = (string)myReader["Turnus"];
                        if (!comboBox1.Items.Contains(dataStreem)) comboBox1.Items.Add(dataStreem);
                    }
                    myReader.Close();

                }
                catch (Exception blad2)
                {
                    MessageBox.Show(blad2.ToString());
                }
                break;

////////////////
                case 0:
                comboBox3.Items.Clear(); // czysci liste wychowawcow

                comboBox2.Items.Clear(); //czysci na wszelki wypadek dane w boxie - zleceniodawcy
                fill = "SELECT Zleceniodawca FROM GLOWNA";
                try
                {

                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();


                    while (myReader.Read())
                    {

                        dataStreem = (string)myReader["Zleceniodawca"];
                        comboBox2.Items.Add(dataStreem);
                    }
                    myReader.Close();
                }
                catch (Exception blad1)
                {
                    MessageBox.Show(blad1.ToString());
                }
                //Wczytywanie danych turnusow  dla comboBox1
                comboBox1.Items.Clear(); //czysci na wszelki wypadek dane w boxie - turnusy
                fill = "SELECT Turnus FROM TURNUSY";

                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        dataStreem = (string)myReader["Turnus"];
                        if (!comboBox1.Items.Contains(dataStreem)) comboBox1.Items.Add(dataStreem);
                    }
                    myReader.Close();

                }
                catch (Exception blad2)
                {
                    MessageBox.Show(blad2.ToString());
                }
                break;

////////////////
                case 3:
                break;
                default :
                MessageBox.Show( "Integral ERROR in CleanUP() ");
                break;
            }
            ///////////////////////// ustawianie poczatkowych wartosci wszystkich 'labelxx'

            textBox5.Text = "Nie wybrano";
            textBox6.Text = "Nie wybrano";
            textBox7.Text = "Nie wybrano";
            textBox8.Text = "0";
            textBox9.Text = "Nie wybrano";
            textBox10.Text = "Nie wybrano";

            textBox1.Clear();
            textBox1.Text = "nie wybrano";
            textBox2.Clear();
            textBox2.Text = "nie wybrano";

            textBox14.Clear();
            textBox15.Clear();

            textBox16.Clear(); // kto powiadomił
            textBox17.Clear(); // data powiadomienia

            textBox4.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();

            checkBox1.Checked = false;
            checkBox2.Checked = false;

        }
        //wypełnia danymi combobox wychowawców
        private void FillUpCommboBox3()
        {
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string dataStreem;

            dataStreem = "";
            fill = "";
            comboBox3.Items.Clear();
            textBox14.Clear();
            textBox15.Clear();
            if (checkBox2.Checked == false) fill = "SELECT Kto FROM WYCHOWAWCY WHERE Turnus = '" + comboBox1.Text + "' AND Zleceniodawca = '" + comboBox2.Text + "'";
            if (checkBox2.Checked == true) fill = "SELECT Kto FROM WYCHOWAWCY WHERE Turnus = '" + comboBox1.Text + "'";
            
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["Kto"];      // wychowawca
                    if (dataStreem != "") comboBox3.Items.Add(dataStreem.Trim());
                    dataStreem = "";
                }
                myReader.Close();
            }
            catch (Exception blad7)
            {
                MessageBox.Show(blad7.ToString());
            }
            if (comboBox3.Items.Count > 0)
            {
                comboBox3.Text = comboBox3.Items[0].ToString(); //ustawia o ile jest wyswietlanie pierwszego rekordu zeznalezionych wychowawcow
                textBox14.Text = comboBox3.Text;
                textBox15.Text = " ";
                fill = "SELECT Telefon FROM WYCHOWAWCY WHERE Kto ='" + comboBox3.Text + "'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        dataStreem = (string)myReader["Telefon"];      // wychowawca
                        textBox15.Text = dataStreem.Trim();
                    }
                    myReader.Close();
                }
                catch (Exception blad8)
                {
                    MessageBox.Show(blad8.ToString());
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
            bool powiadomiono =false;
            // wybrano oba warunki w turnus i zleceniodawca
            fill = "SELECT Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA WHERE Turnus = '" + comboBox1.Text + "'" + "AND Zleceniodawca ='" + comboBox2.Text + "'";
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
                    textBox26.Clear();
                    if (myReader["Wyjazd"] != DBNull.Value)
                    {
                        dataStreem = (string)myReader["Wyjazd"];
                        dataStreem = dataStreem.Replace(tab.ToString(),"");
                        textBox1.Text = dataStreem;
                    }
                    else
                    {
                        textBox1.Text = "";
                    }
                    if (myReader["Powrot"] != DBNull.Value)
                    {
                        dataStreem = (string)myReader["Powrot"];
                        dataStreem = dataStreem.Replace(tab.ToString(),"");
                        textBox26.Text = dataStreem;
                    }
                    else
                    {
                        textBox26.Text = "";
                    }
                    textBox2.Clear();
                    textBox2.Text = (string)myReader["Uwagi"];
                    if (myReader["Organizator"] != DBNull.Value)
                    {
                        dataStreem = (string)myReader["Organizator"];
                        textBox27.Text = dataStreem.Trim();
                    }
                    else
                    {
                        textBox27.Text = "";
                    }
                }

                myReader.Close();
                
                textBox5.Text = comboBox1.Text.Trim();
                textBox7.Text = comboBox2.Text.Trim();
            }
            catch (Exception blad5)
            {
                MessageBox.Show(blad5.ToString());
            }
            dataStreem2 = "";
            fill = "SELECT Od, Do FROM TURNUSY WHERE Turnus = '" + comboBox1.Text + "'";
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
            FillUpCommboBox3();
        }
        //ustawia odpowiednio dane wyświetlane w obu combobox (turnus i zleceniodawca) zaleznie co wybrano (np jak wybrano turnus gąsawa to w combobox zleceniodawca ustawia tylko tych którzy są wpisani na dany turnus)
        private void UstawComboBox(ref ComboBox myComboBox, ref ComboBox myComboBox2, int arg)
        {
            //zmienne wspolne
            // arg == 1 - dla comboBox1 (turnus), arg == 2 dla comboBox2 ( zleceniodawca )
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem;
            string fill;
            bool istnieje;

            if (myComboBox.Text == "                   Pokaż wszystko     ")
            {
                CleanUp( 0 ); //ustawia poczatkowe wartosci pol ,,  arg ??
            }
            else
                if (myComboBox2.Text == "")
                {

                    //Wczytywanie danych zleceniodawcow  dla comboBox (po wybraniu osrodki ustawia tylko tych zleceniodawcow ktorzy sa przypisani do danego osrodka)
                    myComboBox2.Items.Clear();                            //czysci na wszelki wypadek dane w boxie
                    dataStreem = "                   Pokaż wszystko     ";
                    myComboBox2.Items.Add(dataStreem);
                    fill = " ";
                    if (arg == 1)
                    {
                        fill = "SELECT Zleceniodawca FROM GLOWNA WHERE Turnus = '" + comboBox1.Text + "'";
                        dataStreem = "Zleceniodawca";
                    }
                    else
                        if (arg == 2)
                        {
                            fill = "SELECT Turnus FROM GLOWNA WHERE Zleceniodawca = '" + comboBox2.Text + "'";
                            dataStreem = "Turnus";
                        }

                    try
                    {
                        myCommand = new SqlCommand(fill, myCon);
                        myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            //dataStreem = (string)myReader["Zleceniodawca"];
                            myComboBox2.Items.Add( (string)myReader[dataStreem]);
                        }
                        myReader.Close();
                    }
                    catch (Exception blad3)
                    {
                        MessageBox.Show(blad3.ToString());
                    }
                }
                else
                {

                    // wybrano oba warunki w turnus i zleceniodawca ->> spr czy oba sa nadal aktualne !
                    istnieje = false;
                    fill = " ";
                    dataStreem = " ";
                    if (arg == 1)
                    {
                        fill = "SELECT Zleceniodawca FROM GLOWNA WHERE Turnus = '" + comboBox1.Text + "'";
                        dataStreem = "Zleceniodawca";
                    }
                    else
                        if (arg == 2)
                        {
                            fill = "SELECT Turnus FROM GLOWNA WHERE Zleceniodawca = '" + comboBox2.Text + "'";
                            dataStreem = "Turnus";
                        }

                    try
                    {
                        myCommand = new SqlCommand(fill, myCon);
                        myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            //if ((arg == 1) && (string)myReader[dataStreem] == comboBox2.Text) istnieje = true;
                            fill = (string)myReader[dataStreem];

                            if ( fill.Trim() == myComboBox2.Text.Trim()) istnieje = true;
                        }
                        myReader.Close();
                    }
                    catch (Exception blad4)
                    {
                        MessageBox.Show(blad4.ToString());
                    }

                    if (istnieje)
                    {
                        FillUp();
                    }
                    else
                    {
                        if( arg == 1) CleanUp(2);
                        if (arg == 2) CleanUp(1);
                        //Wczytywanie danych zleceniodawcow  dla comboBox2 (po wybraniu osrodki ustawia tylko tych zleceniodawcow ktorzy sa przypisani do danego osrodka)
                        myComboBox2.Items.Clear(); //czysci na wszelki wypadek dane w boxie
                        dataStreem = "                   Pokaż wszystko     ";
                        myComboBox2.Items.Add(dataStreem);
                        fill = " ";
                        dataStreem = " ";
                        if (arg == 1)
                        {
                            fill = "SELECT Zleceniodawca FROM GLOWNA WHERE Turnus = '" + comboBox1.Text + "'";
                            dataStreem = "Zleceniodawca";
                        }
                        else
                            if (arg == 2)
                            {
                                fill = "SELECT Turnus FROM GLOWNA WHERE Zleceniodawca = '" + comboBox2.Text + "'";
                                dataStreem = "Turnus";
                            }

                        //fill = "SELECT Zleceniodawca FROM GLOWNA WHERE Turnus = '" + comboBox1.Text + "'";
                        try
                        {
                            myCommand = new SqlCommand(fill, myCon);
                            myReader = myCommand.ExecuteReader();
                            while (myReader.Read())
                            {
                                myComboBox2.Items.Add((string)myReader[dataStreem]);
                            }
                            myReader.Close();
                        }
                        catch (Exception blad6)
                        {
                            MessageBox.Show(blad6.ToString());
                        }
                    }

                }
            
        }
        //zapisuje zmiany zleceniodawcy i wychowawcy o ile były zmienione
        private void ZapiszZmiany()
        {
            //zmienne wspolne

            SqlCommand myCommand;
            string fill;
            int powiadomiono = 0;
            string turnus = textBox5.Text.Trim(); ;
            string zleceniodawca = textBox7.Text.Trim();

            if ( (comboBox1.Text != "") && (comboBox2.Text != ""))
            {


            if (zmieniono)
            {
            if (checkBox1.Checked) powiadomiono = 1;
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
            Powrot ='" + textBox26.Text + @"',
            Uwagi ='" + textBox2.Text + @"',
            Data_powiadom ='" + textBox17.Text + @"',
            Turnus ='" + textBox5.Text + @"',
            Organizator ='" + textBox27.Text + @"'
            WHERE 
                Turnus ='" + comboBox1.Text + @"' 
            AND 
                Zleceniodawca ='" + comboBox2.Text + @"' ";

                    try
                    {
                        //MessageBox.Show(fill);
                        myCommand = new SqlCommand(fill, myCon);
                        myCommand.ExecuteNonQuery();
                    }
                    catch (Exception blad1)
                    {
                        MessageBox.Show(blad1.ToString());
                    }
                /*
                string text = toolStripStatusLabel1.Text.Substring(22, (toolStripStatusLabel1.Text.Length-22)); //statusStrip1.Text.Substring(11,(statusStrip1.Text.Length-11));
                fill = "INSERT INTO ZMIANY (Kto, Id_zlec, Nazwa_zlec, Turnus, Zmiana) VALUES ('" + text + "', 0, '" + comboBox2.Text + "', '" + comboBox1.Text + "', '" + zmiany + "')";
                    try
                    {
                        //MessageBox.Show(fill);
                        myCommand = new SqlCommand(fill, myCon);
                        myCommand.ExecuteNonQuery();
                    }
                    catch (Exception blad1)
                    {
                        MessageBox.Show(blad1.ToString());
                    }
                */

            }

            if ((zmienionoWychowawce == true) || (textBox5.Text != comboBox1.Text.Trim()) )
            {

                if (textBox5.Text != comboBox1.Text.Trim()) fill = "UPDATE WYCHOWAWCY SET Kto='" + textBox14.Text + "', Telefon ='" + textBox15.Text + "', Zleceniodawca ='' WHERE Kto ='" + comboBox3.Text + "'";
                else fill = "UPDATE WYCHOWAWCY SET Kto='" + textBox14.Text +"', Telefon ='" + textBox15.Text +"' WHERE Kto ='"+ comboBox3.Text +"'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad2)
                {
                    MessageBox.Show(blad2.ToString());
                }

            }

            for (int t = 0; t < 12; t++) zmiany_bool[t] = false;
            zmieniono = false;
            zmienionoWychowawce = false;
            button4.Enabled = false;
            groupBox1.Enabled = true;
            CleanUp(0);
            powiadomiono = 0;

            //for(int count =0; count < comboBox1.c)
            //if (comboBox1.Items.Contains(turnus)) MessageBox.Show("znaleziono na liscie -" + turnus);
            powiadomiono = comboBox1.FindString(turnus,0);
            if( (powiadomiono >= 0 ) && (powiadomiono < comboBox1.Items.Count) )comboBox1.SelectedIndex = powiadomiono;

            powiadomiono = comboBox2.FindString(zleceniodawca,0);
            if ((powiadomiono >= 0) && (powiadomiono < comboBox2.Items.Count)) comboBox2.SelectedIndex = powiadomiono;

            if( ( comboBox1.Text != "" ) && ( comboBox2.Text != "" ) )FillUp();
            
            
            }
            else MessageBox.Show("Nie wprowadzono wszystkich danych - nie można zapisac!");

        }
        //drukowanie strony
        private void printDoc_PrintPage(Object sender,PrintPageEventArgs e)
        {
            //string powiadomiony = "";
            //string wszyscyWych = "";
            //string pomoc;
            //int count = 0;
            //int rest = 0;
            //char[] temp = new char[106];
            /*
            wszyscyWych = textBox2.Text.Trim();
            if (wszyscyWych.Length > 105)
            {
                count = wszyscyWych.Length / 105;
                rest = wszyscyWych.Length % 105;
                for (int z = 0; z < count; z ++)
                  {
                        wszyscyWych.CopyTo(z * 105, temp, 0, 105);
                        pomoc = new string(temp);
                        powiadomiony += pomoc + "\n";
                  }
                wszyscyWych.CopyTo(105*count,temp,0,rest);
                pomoc = new string(temp,0,rest);
                powiadomiony += pomoc + "\n";
                wszyscyWych = powiadomiony;

            }
            if( checkBox1.Checked == true ) powiadomiony = "TAK"; else powiadomiony = "NIE";
            /*
            string textToPrint =
                "\n \n \n" +
                "Turnus: " + comboBox1.Text.Trim() + "     data: " + textBox6.Text.Trim() + "\n" +
                "Zleceniodawca: " + comboBox2.Text.Trim() + "\n" +
                "Powiadomiony: " + powiadomiony + "    przez: " + textBox16.Text.Trim() + "    dnia: " + textBox17.Text.Trim() + "\n" +
                "Ilość dzieci: " + textBox8.Text + "\n" +
                "Osoba kontaktowa: " + textBox9.Text + "\n" +
                "tel. " + textBox10.Text + "\n" +
                "\n" +
                "Adres zleceniodawcy :\n" +
                "ul. " + textBox4.Text.Trim() + "  " + textBox13.Text.Trim() + "\n" +
                "" + textBox11.Text.Trim() + "  " + textBox12.Text.Trim() + "\n" +
                "\n" +
                "TRANSPORT :\n Wyjazd - " + textBox1.Text.Trim() + "\n Powrót - " + textBox26.Text.Trim() + "\n" +
                "\n" +
                "UWAGI :\n" + wszyscyWych + "\n" +
                "\n";

            wszyscyWych = "";
            if (checkBox2.Checked == true) wszyscyWych = "(wszyscy na tym turnusie)";
            textToPrint += "Wychowawcy:" + wszyscyWych + "\n";
            for (int i = 0; i < comboBox3.Items.Count; i++) //w petli dodaje wychowawcow
            {
                powiadomiony = comboBox3.Items[i].ToString();
                textToPrint += "                             " + powiadomiony.Trim() + "\n";
            }
            */
            string textToPrint =
                "\n \n \n" 
                + comboBox1.Text.Trim() + "     data: " + textBox6.Text.Trim() + "\n" +
                "Zleceniodawca: " + comboBox2.Text.Trim() + "\t" + "Ilość dzieci: " + textBox8.Text + "\n" +
                "\n Wyjazd \n" + textBox1.Text + "\n\n Powrót \n" + textBox26.Text + "\n\n" +
                "UWAGI :\n" + textBox2.Text + "\n";

            Font printFont = new Font("Times New Roman", 10);
            e.Graphics.DrawString(textToPrint, printFont,Brushes.Black, 30, 30);
        }
        //czynności wykonywane gdy zamykamy program
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myCon != null) 
            {
                if (zmieniono)
                {
                    string message = "  Zapisać zmiany ?  ";
                    string caption = "Ostrzeżenia";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                    DialogResult result;
                    result = MessageBox.Show(this, message, caption, buttons);
                    if( result == DialogResult.Yes ) ZapiszZmiany();
                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                myCon.Close();
                //MessageBox.Show( "Zamknięto sql connection");
            }
        }
        //reakcja na wybranie z rozwijanego menu pozycji - Serwer Bazy
        private void serwerBazyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction...");
            return;
            //wymaga poprawek
            Form2 dlg = new Form2();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                StreamWriter write = new StreamWriter("setup.ini");
                write.WriteLine( dlg.textBox1.Text);
                write.Close();
            }

        }
        //reakcja na wybranie z rozwijanego menu pozycji -> create base
        private void createBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //aktualnie plik jest szyfrowany wiec to nie dziala.....
            MessageBox.Show("Under construction...");
            return;
            DialogResult result = new DialogResult();
            Form5 dlg = new Form5();
            result = dlg.ShowDialog();
            if (result == DialogResult.OK)
                if( dlg.textBox1.Text !="")
                {
                    SqlCommand myCommand;
                    string fill;
              
                    fill = dlg.textBox1.Text;
                    try
                    {
                        myCommand = new SqlCommand(fill, myCon);
                        myCommand.ExecuteNonQuery();
                    }
                    catch (Exception blad1)
                    {
                        MessageBox.Show(blad1.ToString());
                    }
                    MessageBox.Show("Wykonano");
                }
            
        }
        
        //reakcja na wybranie nowej pozycji z combobox turnusy
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (combo1SelectedIndex == -1) UstawComboBox(ref comboBox1, ref comboBox2, 1);
            else
            {
                comboBox1.SelectedIndex = combo1SelectedIndex;
                combo1SelectedIndex = -1;
            }

        }
        private void comboBox1_DropDown(object sender, EventArgs e)
        {

            if (zmieniono && (comboBox1.Text != "") && (comboBox2.Text != ""))
            {
                string message = "Zmieniono dane zleceniodawcy. Zapisać ?";
                string caption = "Zmiany";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result;
                result = MessageBox.Show(this, message, caption, buttons);

                switch (result)
                {
                    case DialogResult.No:
                        zmieniono = false;
                        button4.Enabled = false;
                        groupBox1.Enabled = true;
                        combo1SelectedIndex = -1;
                        //UstawComboBox(ref comboBox1, ref comboBox2, 1);
                        break;

                    case DialogResult.Yes:
                        ZapiszZmiany();
                        zmieniono = false;
                        button4.Enabled = false;
                        groupBox1.Enabled = true;
                        combo1SelectedIndex = -1;
                        //UstawComboBox(ref comboBox1, ref comboBox2, 1);
                        break;
                    case DialogResult.Cancel:
                        combo1SelectedIndex = comboBox1.SelectedIndex;
                        break;
                }

            }
            else
            {
                zmieniono = false;
                button4.Enabled = false;
            }
            
        }
        //reqakcja na wybranie nowej pozycji z combobox zleceniodawcy
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo2SelectedIndex == -1) UstawComboBox(ref comboBox2, ref comboBox1, 2);
            else
            {
                comboBox2.SelectedIndex = combo2SelectedIndex;
                combo2SelectedIndex = -1;
            }
                
        }
        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            if (zmieniono && (comboBox1.Text != "") && (comboBox2.Text != ""))
            {
                string message = "Zmieniono dane zleceniodawcy. Zapisać ?";
                string caption = "Zmiany";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result;
                result = MessageBox.Show(this, message, caption, buttons);

                switch (result)
                {
                    case DialogResult.No:
                        zmieniono = false;
                        button4.Enabled = false;
                        groupBox1.Enabled = true;
                        combo2SelectedIndex = -1;
                    break;

                    case DialogResult.Yes:
                        ZapiszZmiany();
                        zmieniono = false;
                        button4.Enabled = false;
                        groupBox1.Enabled = true;
                        combo2SelectedIndex = -1;
                        //UstawComboBox(ref comboBox1, ref comboBox2, 1);
                    break;

                    case DialogResult.Cancel:
                    combo2SelectedIndex = comboBox2.SelectedIndex;
                    break;
                }

            }
            else
            {
                zmieniono = false;
                button4.Enabled = false;
            }
        }
        //reakcja na wybranie nowej pozycji z combobox wychowawcy
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string dataStreem;

            textBox14.Text = comboBox3.Text.Trim();
            fill = "SELECT Telefon FROM WYCHOWAWCY WHERE Kto ='" + comboBox3.Text + "'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        dataStreem = (string)myReader["Telefon"];      // wychowawca
                        textBox15.Text = dataStreem.Trim();
                    }
                    myReader.Close();
                }
                catch (Exception blad8)
                {
                    MessageBox.Show(blad8.ToString());
                }
        }
        
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string dataStreem;

            if ((checkBox2.Checked == true) && (comboBox1.Text != "") && (comboBox1.Text != "                   Pokaż wszystko     ") )
            {
                //MessageBox.Show("jestem w srodku");
                comboBox3.Items.Clear();
                fill = "SELECT Kto FROM WYCHOWAWCY WHERE Turnus ='" + comboBox1.Text + "'";
                try
                {
                    
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        dataStreem = (string)myReader["Kto"];      // wychowawca
                        dataStreem.Trim();
                        comboBox3.Items.Add( dataStreem );
                    }
                    myReader.Close();
                }
                catch (Exception blad1)
                {
                    MessageBox.Show(blad1.ToString());
                }
                if (comboBox3.Items.Count > 0)
                {
                    comboBox3.Text = comboBox3.Items[0].ToString();
                    textBox14.Text = comboBox3.Text.Trim();
                    
                    fill = "SELECT Telefon FROM WYCHOWAWCY WHERE Kto ='" + comboBox3.Text + "'";
                    try
                    {
                        myCommand = new SqlCommand(fill, myCon);
                        myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            dataStreem = (string)myReader["Telefon"];      // wychowawca
                            textBox15.Text = dataStreem.Trim();
                        }
                        myReader.Close();
                    }
                    catch (Exception blad2)
                    {
                        MessageBox.Show(blad2.ToString());
                    }
                }
            }
            else
            {
                //MessageBox.Show("jestem w else");
                if (checkBox2.Checked == false)
                {
                    comboBox3.Items.Clear();
                    fill = "SELECT Kto FROM WYCHOWAWCY WHERE Turnus ='" + comboBox1.Text + "' AND Zleceniodawca ='" + comboBox2.Text + "'";
                    try
                    {
                        
                        myCommand = new SqlCommand(fill, myCon);
                        myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            dataStreem = (string)myReader["Kto"];      // wychowawca
                            dataStreem.Trim();
                            comboBox3.Items.Add(dataStreem);
                        }
                        myReader.Close();
                    }
                    catch (Exception blad3)
                    {
                        MessageBox.Show(blad3.ToString());
                    }
                    if (comboBox3.Items.Count > 0)
                    {
                        comboBox3.Text = comboBox3.Items[0].ToString();
                        textBox14.Text = comboBox3.Text.Trim();

                        fill = "SELECT Telefon FROM WYCHOWAWCY WHERE Kto ='" + comboBox3.Text + "'";
                        try
                        {
                            myCommand = new SqlCommand(fill, myCon);
                            myReader = myCommand.ExecuteReader();
                            while (myReader.Read())
                            {
                                dataStreem = (string)myReader["Telefon"];      // wychowawca
                                textBox15.Text = dataStreem.Trim();
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
        }
        // dodaj nowego zleceniodawce
        private void button1_Click(object sender, EventArgs e)
        {
            if (zmieniono)
            {


                string message = "Przed przejściem do wprowadzania nowego wpisu, \n należy zapisac zmiany. Zapisać teraz ?";
                string caption = "Ostrzeżenie";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result;
                result = MessageBox.Show(this, message, caption, buttons);

                switch (result)
                {
                    case DialogResult.No:
                        zmieniono = false;
                        button4.Enabled = false;
                        
                        Form4 dlg = new Form4(myCon);
                        DialogResult arg;
                        arg = dlg.ShowDialog();
                        if (arg == DialogResult.OK)
                        {
                            UstawComboBox(ref comboBox2, ref comboBox1, 1);
                        }
                        //UstawComboBox(ref comboBox1, ref comboBox2, 1);
                        break;

                    case DialogResult.Yes:
                        ZapiszZmiany();
                        zmieniono = false;
                        button4.Enabled = false;

                        Form4 dlg2 = new Form4(myCon);
                        DialogResult arg2;
                        arg2 = dlg2.ShowDialog();

                        if (arg2 == DialogResult.OK)
                        {
                            UstawComboBox(ref comboBox2, ref comboBox1, 1);
                        }

                        //UstawComboBox(ref comboBox1, ref comboBox2, 1);
                        break;
                    case DialogResult.Cancel:

                        return;
                }
                //MessageBox.Show("Czy zapisać zmiany ??");
            }
            else
            {

                Form4 dlg = new Form4(myCon);
                DialogResult arg;
                arg = dlg.ShowDialog();
                
                if (arg == DialogResult.OK)
                {
                    UstawComboBox(ref comboBox2, ref comboBox1, 1);
                }
                else
                    if ( arg == DialogResult.Cancel)
                    {
                        ;// MessageBox.Show("ze niby anulowano");
                    }
                    else
                    {
                        ;// MessageBox.Show("cos nie tak ");
                    }
                
            }

        }
        // wyszukiwanie
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)  // 13 is the ASCII value for a carriage return
            {
                SqlDataReader myReader;
                SqlCommand myCommand;
                string dataStreem;
                string fill;

                comboBox2.Items.Clear();
                dataStreem = "                   Pokaż wszystko     ";
                comboBox2.Items.Add(dataStreem);

                fill = "SELECT * FROM GLOWNA WHERE Zleceniodawca LIKE '%" + textBox3.Text + "%'";
                dataStreem = "Zleceniodawca";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        comboBox2.Items.Add((string)myReader[dataStreem]);
                    }
                    myReader.Close();
                    if (comboBox2.Items.Count > 1) comboBox2.Text = comboBox2.Items[1].ToString();
                    else MessageBox.Show("Nie znaleziono zleceniodawcy\n zawierającego w nazwie : " + textBox3.Text );
                    textBox3.Clear();
                    //UstawComboBox(ref comboBox2, ref comboBox1, 2);
                }
                catch (Exception blad6)
                {
                    MessageBox.Show(blad6.ToString());
                }

            }
        }
        //zamkniecie wyboru z kalendarza - wybrano jakas date
        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            DateTime dataWyb = dateTimePicker1.Value;

            if (textBox17.Text != "")
            {
                textBox17.Text = dataWyb.Date.ToString("dd.MM.yyyy");
                zmieniono = true;
                button4.Enabled = true;
            }
        }
        //sumuje ilosc dzieci na turnusie / all
        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem;
            string fill;
            int count_turnus = 0;
            int count_all = 0;


            // zlicza ilosc dzieci na turnusie
            if ((comboBox1.Text != "") && (comboBox2.Text != ""))
            {
                fill = "";
                if (comboBox1.Text != "") fill = "SELECT Il_dzieci FROM GLOWNA WHERE Turnus ='" + comboBox1.Text + "'";
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
            dataStreem = "Ilość dzieci na turnusie :" + comboBox1.Text.Trim() + "   === " + count_turnus.ToString() + "\n";
            fill = "Ilość dzieci na całych koloniach/feriach === " + count_all.ToString();
            dataStreem = (dataStreem + fill);
            MessageBox.Show(dataStreem);

        }
        // zmieniono stan powiadomienia
        private void checkBox1_Click(object sender, EventArgs e)
        {
            DateTime dataAkt = DateTime.Now;

            if ((checkBox1.Checked == true))
                if ((comboBox1.Text != "") && (comboBox1.Text != "                   Pokaż wszystko     ") && (comboBox2.Text != "") && (comboBox2.Text != "                   Pokaż wszystko     "))
                {
                    textBox16.Text = "Karol Ney";
                    textBox17.Text = dataAkt.Date.ToString("dd.MM.yyyy");
                    zmieniono = true;
                    button4.Enabled = true;
                }
                else
                {
                    checkBox1.Checked = false;

                }


            if ((checkBox1.Checked == false) && (comboBox1.Text != "") && (comboBox1.Text != "                   Pokaż wszystko     ") && (comboBox2.Text != "") && (comboBox2.Text != "                   Pokaż wszystko     "))
            {
                textBox16.Clear();
                textBox17.Clear();
                zmieniono = true;
                button4.Enabled = true;
                //MessageBox.Show("jestem w srodku, sa wybrane turnus i gmina ale odznaczam wybor");
            }
        }
        //zmieniono styl wyswietlania wychowawcow
        private void checkBox2_Click(object sender, EventArgs e)
        {
            if ((checkBox2.Checked) && (comboBox1.Text != "") && (comboBox2.Text != "") && (!zmieniono))
            {
                FillUp();
            }
            else
                if ((!checkBox2.Checked) && (!zmieniono) && (comboBox1.Text != "") && (comboBox2.Text != ""))
                {
                    FillUp();
                }
                else
                {
                    checkBox2.Checked = false;
                }
        }
        // button usun - zleceniodawce
        private void button9_Click(object sender, EventArgs e)
        {

            if ((comboBox1.Text != "") && (comboBox2.Text != ""))
            {

                string message = "Usunąc zleceniodawce : " + comboBox2.Text.Trim() + " ?";
                string caption = "Ostrzeżenia";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(this, message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    if ( IstniejeZleceniodawca(comboBox2.Text, comboBox1.Text) )
                    {
                        ClearWych( comboBox2.Text.Trim(), comboBox1.Text.Trim() );//usuwa w wychowawcach wpisy o takim zleceniodawcuy jezeli ktos byl na niego wpisany
                        //zmienne wspolne
                        SqlCommand myCommand;
                        string fill;
                        bool err = false;

                        fill = "DELETE FROM GLOWNA WHERE Zleceniodawca ='" + comboBox2.Text + "' AND Turnus ='" + comboBox1.Text + "'";
                        try
                        {
                            myCommand = new SqlCommand(fill, myCon);
                            myCommand.ExecuteNonQuery();
                        }
                        catch (Exception blad4)
                        {
                            err = true;
                            fill = blad4.ToString();
                        }
                        if (err) MessageBox.Show("Wystąpił problem z usunięciem rekordu\n" + fill);
                        else MessageBox.Show("Usunięto poprawnie zleceniodawce");
                    }
                    else MessageBox.Show("Nie ma w bazie takiego zleceniodawcy ^^");

                }
                if (result == DialogResult.Cancel)
                {
                    return;
                }


            }
            else
            {
                MessageBox.Show("Nie można skasować , musi być wybrane Turnus i Zleceniodawca");
            }

        }
        //button przydziel wychowawce
        private void button10_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Istotne tylko dla zmiany zapis button
        /// </summary>
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
                zmiany += "Zmieniono ilość dzieci \n";
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
                zmiany += "Zmieniono telefon kontaktowy \n";
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
                zmiany += "Zmieniono adres zlec. - kod pocz. \n";
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
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[7])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono wyjazd grupy \n";
                zmiany_bool[7] = true;
            }

        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[8])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono - uwagi \n";
                zmiany_bool[8] = true;
            }

        }
        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[9])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono adres zlec. - nr. lok. \n";
                zmiany_bool[9] = true;
            }
        }
        private void textBox26_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[10])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono powrót grupy \n";
                zmiany_bool[10] = true;
            }
        }
        // zmiana w nazwie wych.
        private void textBox14_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            zmieniono = true;
            button4.Enabled = true;
            zmienionoWychowawce = true;

        }
        //zmiana tel. wychowawcy
        private void textBox15_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            zmieniono = true;
            button4.Enabled = true;
            zmienionoWychowawce = true;
        }
        //zmiany organizator
        private void textBox27_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!zmiany_bool[11])
            {
                zmieniono = true;
                button4.Enabled = true;
                zmiany += "Zmieniono organizatora \n";
                zmiany_bool[11] = true;
            }
        }
        //proba zmiany turnusu na ktorym jest zleceniodawca
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox3.Items.Count > 0)
            {
                MessageBox.Show( "Uwaga !\nJeżeli zmienisz turnus wychowawcy automatycznie otrzymają status\n< bez transportu > i pozostaną na starym turnusie" );
            }

            if ((comboBox1.Text != "") && (comboBox2.Text != ""))
            {
                Form6 dlg = new Form6();
                DialogResult wynik;
                wynik = dlg.ShowDialog();
                if ((wynik == DialogResult.OK) && (dlg.comboBox1.Text != "") && ( dlg.comboBox1.Text.Trim() != comboBox1.Text.Trim()))
                {
                    //MessageBox.Show(dlg.comboBox1.Text);
                    textBox5.Clear();
                    textBox5.Text = dlg.comboBox1.Text.Trim();
                    textBox6.Text = dlg.label2.Text;
                    zmieniono = true;
                    button4.Enabled = true;
                    groupBox1.Enabled = false;
                    
                }
                else
                {
                    textBox5.Clear();
                    textBox5.Text = comboBox1.Text.Trim();

                }
            }

        }
        // button cofnij zmiany
        private void button16_Click(object sender, EventArgs e)
        {
            if (zmieniono || zmienionoWychowawce)
            {
                FillUp();
                zmieniono = false;
                zmienionoWychowawce = false;
                button4.Enabled = false;
                groupBox1.Enabled = true;
            }

        }
        //zapisz zmiany zleceniodawcy
        private void button4_Click(object sender, EventArgs e)
        {
            ZapiszZmiany();
        }
        //drukowanie
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
        //page preview
        private void button5_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.SetDesktopBounds(0,0,600,600);
            dlg.Document = printDoc;
            dlg.ShowDialog();

        }
        // przejscie na zakladke glowna
        private void tabPage1_Enter(object sender, EventArgs e)
        {
            if ((tab1Leave.turnus != "") && (tab1Leave.zleceniodawca != ""))
            {

            }
            else
            {
                CleanUp(0); //ustawia poczatkowe wartosci dla wszystkich pol
            }
        }
        //przejście na inną zakładkę ( z zakładki główna )
        private void tabPage1_Leave(object sender, EventArgs e)
        {

            if( comboBox1.SelectedIndex != -1 )
            {

                tab1Leave.turnus = comboBox1.Text;
            }
            if (comboBox2.SelectedIndex != -1)
            {

                tab1Leave.zleceniodawca = comboBox2.Text;
            }

        }

        

 
        ////////////////////////////       ZMIANY i EVENTY DOTYCZACE ZAKŁADKI TURNUSY
            

        //spr. czy istnieje podany turnus

        private bool IstniejeTurnus( string Turnus )
        {

            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem = "";
            string fill;
            int count = 0;

            fill = "SELECT Turnus FROM TURNUSY WHERE Turnus ='" + Turnus + "'";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["Turnus"];
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
        //spr. czy istnieje juz podany ośrodek
        private bool IstniejeOsrodek(string Osrodek)
        {

            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem = "";
            string fill;
            int count = 0;

            fill = "SELECT Osrodek FROM OSRODKI WHERE Osrodek ='" + Osrodek + "'";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["Osrodek"];
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
        // ustawia listView dla turnusów
        private void FillListView()
        {
            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string[] dataStreem = new string[6];
            string fill;
            int count = 0;

            listView1.Items.Clear();
            fill = "SELECT Turnus, Od, Do, il_wych, il_dzieci, Kierownik FROM TURNUSY";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ListViewItem lvi = new ListViewItem();
                    dataStreem[1] = (string)myReader["Od"];
                    dataStreem[2] = (string)myReader["Do"];
                    dataStreem[0] = (string)myReader["Turnus"];
                    count = (int)myReader["il_wych"];
                    dataStreem[3] = count.ToString();
                    count = (int)myReader["il_dzieci"];
                    dataStreem[4] = count.ToString();
                    dataStreem[5] = (string)myReader["Kierownik"];
                    
                    lvi.Text = dataStreem[0].Trim();
                    lvi.SubItems.Add(dataStreem[1].Trim());
                    lvi.SubItems.Add(dataStreem[2].Trim());
                    lvi.SubItems.Add(dataStreem[3].Trim());
                    lvi.SubItems.Add(dataStreem[4].Trim());
                    lvi.SubItems.Add(dataStreem[5].Trim());
                    listView1.Items.Add(lvi);
                }
                myReader.Close();
            }
            catch (Exception blad4)
            {
                MessageBox.Show(blad4.ToString());
            }

        }
        // ustawia listBox dla osrodków
        private void FillListBox()
        {
            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem;
            string dataStreem2;
            string fill;

            tab2ListBox = new ArrayList();
            listBox1.Items.Clear();
            fill = "SELECT Osrodek, Nazwa FROM OSRODKI";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["Osrodek"];
                    dataStreem2 = (string)myReader["Nazwa"];
                    listBox1.Items.Add( (dataStreem.Trim() + " - " + dataStreem2.Trim() ) );
                    tab2ListBox.Add(dataStreem);
                }
                myReader.Close();
            }
            catch (Exception blad4)
            {
                MessageBox.Show(blad4.ToString());
            }
        }
        // funkcja usuwająca turnus
        private void UsunTurnus(string Turnus)
        {
            if (IstniejeTurnus(Turnus))
            {
                //zmienne wspolne
                SqlCommand myCommand;
                string fill;
                bool err = false;

                SqlDataReader myReader;
                string dataStreem;
                ArrayList myTurnus = new ArrayList();
                int i=0;

                fill = "SELECT Turnus, Zleceniodawca FROM GLOWNA WHERE Turnus ='" + Turnus +"'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        dataStreem = (string)myReader["Zleceniodawca"];
                        myTurnus.Add( dataStreem );
                    }
                    myReader.Close();
                }
                catch (Exception blad1)
                {
                    MessageBox.Show(blad1.ToString());
                }

                if (myTurnus.Count > 0)
                {
                    dataStreem = "";
                    for (i = 0; i < myTurnus.Count; i++)
                    {
                        fill = myTurnus[i].ToString();
                        dataStreem += fill.Trim();
                        dataStreem += ", ";
                        if ( ( (i % 3) == 0) && (i !=0) ) dataStreem += "\n";
                    }
                    MessageBox.Show("Nie można usunąc Turnusu\nSą już na nim zapisani zleceniodawcy :\n" + dataStreem );
                    return;
                }

                fill = "DELETE FROM TURNUSY WHERE Turnus ='" + Turnus + "'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad4)
                {
                    err = true;
                    fill = blad4.ToString();
                }
                if (err) MessageBox.Show("Wystąpił problem z usunięciem rekordu\n" + fill);
                else MessageBox.Show("Usunięto poprawnie Turnus");
                FillListView();
            }
            else MessageBox.Show("Nie ma w bazie takiego turnusa");
        }
        // funkcja dodajaca turnus
        private void AddTurnus( string Turnus, string dataOd, string dataDo, int il_wych, int il_dzieci, string kierownik)
        {
            //zmienne wspolne
            SqlCommand myCommand;
            string fill;
            bool err = false;

            if (IstniejeTurnus(Turnus))
            {
                fill = "UPDATE TURNUSY SET Od ='" + dataOd + "', Do ='" + dataDo + "', il_wych =" + il_wych + ", il_dzieci =" + il_dzieci + ", Kierownik ='" + kierownik + "'  WHERE Turnus ='" + Turnus + "'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad2)
                {
                    err = true;
                    fill = blad2.ToString();
                }
                if (err) MessageBox.Show("Wystąpił problem ze zmianą rekordu\n" + fill);
                else MessageBox.Show("Zmieniono dane turnusu");
                FillListView();
                
            }
            else
            {
                fill = "INSERT INTO TURNUSY (Turnus, Od, Do, il_wych, il_dzieci, Kierownik) VALUES ('" + Turnus + "','" + dataOd + "','" + dataDo + "'," + il_wych + "," + il_dzieci + ",'" + kierownik + "')";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad4)
                {
                    err = true;
                    fill = blad4.ToString();
                }
                if (err) MessageBox.Show("Wystąpił problem z dodaniem rekordu\n" + fill);
                else MessageBox.Show("Dodano poprawnie Turnus");
                FillListView();
                
            }
        }
        // funkcja usuwajaca osrodek
        private void UsunOsrodek(string Osrodek)
        {
            if (IstniejeOsrodek(Osrodek))
            {
                //zmienne wspolne
                SqlCommand myCommand;
                string fill;
                bool err = false;

                fill = "DELETE FROM OSRODKI WHERE Osrodek ='" + Osrodek + "'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad4)
                {
                    err = true;
                    fill = blad4.ToString();
                }
                if (err) MessageBox.Show("Wystąpił problem z usunięciem rekordu\n" + fill);
                else MessageBox.Show("Usunięto poprawnie Ośrodek");
                FillListBox();
            }
            else MessageBox.Show("Nie ma w bazie takiego ośrodka");
        }
        // funkcja dodajaca osrodek
        private void AddOsrodek(string[] Osrodek)
        {
            //zmienne wspolne
            SqlCommand myCommand;
            string fill;
            bool err = false;

            if (IstniejeOsrodek(Osrodek[0]))
            {
                fill = "UPDATE OSRODKI SET Nazwa ='" + Osrodek[1] + "', Tel ='" + Osrodek[2] + "', Ul ='" + Osrodek[3] + "', Nr_lokalu ='" + Osrodek[4] + "', Kod_pocz ='" + Osrodek[5] + "', Poczta ='" + Osrodek[6] + "' WHERE Osrodek ='" + Osrodek[0] + "'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad2)
                {
                    err = true;
                    fill = blad2.ToString();
                }
                if (err) MessageBox.Show("Wystąpił problem ze zmianą rekordu\n" + fill);
                else MessageBox.Show("Zmieniono dane ośrodka");
                FillListBox();

            }
            else
            {
                fill = "INSERT INTO OSRODKI (Osrodek, Nazwa, Tel, Ul, Nr_lokalu, Kod_pocz, Poczta) VALUES ('" + Osrodek[0] + "','" + Osrodek[1] + "','" + Osrodek[2] + "','" + Osrodek[3] + "','" + Osrodek[4] + "','" + Osrodek[5] + "','" + Osrodek[6] + "')";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad4)
                {
                    err = true;
                    fill = blad4.ToString();
                }
                if (err) MessageBox.Show("Wystąpił problem z dodaniem rekordu\n" + fill);
                else MessageBox.Show("Dodano poprawnie Ośrodek");
                FillListBox();

            }
        }
        // ustawia poczatkowe wartosci przy inicjalizacji zakladki - kiedy user przejdzie poraz pierwszy na nia
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            FillListView();
            FillListBox();
            button18.Enabled = false;
        }
        // czysc - button <czysc>
        private void button15_Click(object sender, EventArgs e)
        {
            textBox18.Clear();
            textBox19.Clear();
            textBox20.Clear();
            textBox35.Clear();
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }
        // usun turnus - button <usun>
        private void button7_Click(object sender, EventArgs e)
        {
            if ( (textBox18.Text != "") ) UsunTurnus( textBox18.Text );

        }
        // wybrano inny turnus ustaw zmienne
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( (listView1.SelectedItems.Count > 0) )
            {
                //MessageBox.Show("cos nowego wybrales :" + listView1.SelectedItems[0].SubItems[0].Text);
                textBox18.Text = listView1.SelectedItems[0].SubItems[0].Text; //nazwa turnusa
                textBox19.Text = listView1.SelectedItems[0].SubItems[1].Text; // data od
                textBox20.Text = listView1.SelectedItems[0].SubItems[2].Text; // data do
                textBox35.Text = listView1.SelectedItems[0].SubItems[5].Text; //kierownik
                numericUpDown1.Value = Convert.ToInt16(listView1.SelectedItems[0].SubItems[3].Text); //il wych.
                numericUpDown2.Value = Convert.ToInt16(listView1.SelectedItems[0].SubItems[4].Text); //il dzieci

            }
            //else MessageBox.Show("nic nie wybrano");
        }
        // dodaje/zmienia turnus po kliknieciu button <dodaj/zapisz> 
        private void button6_Click(object sender, EventArgs e)
        {
            if ((zmienionoTab2) &&(textBox18.Text != "") && (textBox19.Text != "") && (textBox20.Text != ""))
            {
                AddTurnus( textBox18.Text, textBox19.Text, textBox20.Text, Convert.ToInt16(numericUpDown1.Value), Convert.ToInt16(numericUpDown2.Value), textBox35.Text);
            }
        }
        //wybór daty od jakiej ma się rozpoczynać turnus
        private void dateTimePicker2_CloseUp(object sender, EventArgs e)
        {
            DateTime dataWyb = dateTimePicker2.Value;
            textBox19.Text = dataWyb.Date.ToString("dd.MM.yyyy");
            zmienionoTab2 = true;
        }
        //wybór daty do jakiej ma trwać turnus
        private void dateTimePicker3_CloseUp(object sender, EventArgs e)
        {
            DateTime dataWyb = dateTimePicker3.Value;
            textBox20.Text = dataWyb.Date.ToString("dd.MM.yyyy");
            zmienionoTab2 = true;
        }
        // 6 poniższych funkcji ustawia tylko flagę ze zmieniono dane turnusa -> daje znac ze cos trzeba by zapisac xD
        private void textBox35_KeyPress(object sender, KeyPressEventArgs e)
        {
            zmienionoTab2 = true;
        }
        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            zmienionoTab2 = true;
        }
        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            zmienionoTab2 = true;
        }
        private void textBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            zmienionoTab2 = true;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            zmienionoTab2 = true;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            zmienionoTab2 = true;
        }


        ////////////  Tyczy sie czesci zwiazanej z osrodkami

        // wybrano inny osrodek z listy -> ustawia wszystkie dane
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //textBox24.Text = listBox1.Items[listBox1.].ToString();
            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string dataStreem;
            string fill;

            fill = "SELECT Osrodek, Nazwa, Ul, Nr_lokalu, Kod_pocz, Poczta, Tel FROM OSRODKI WHERE Osrodek ='" + tab2ListBox[ listBox1.SelectedIndex ].ToString() + "'";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem = (string)myReader["Osrodek"];
                    textBox24.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Nazwa"];
                    textBox23.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Tel"];
                    textBox34.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Ul"];
                    textBox22.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Nr_lokalu"];
                    textBox25.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Kod_pocz"];
                    textBox32.Text = dataStreem.Trim();
                    dataStreem = (string)myReader["Poczta"];
                    textBox33.Text = dataStreem.Trim();
                }
                myReader.Close();
            }
            catch (Exception blad4)
            {
                MessageBox.Show(blad4.ToString());
            }
            button18.Enabled = true;
            button19.Enabled = true;

        }
        // dodawanie ośrodka
        private void button19_Click(object sender, EventArgs e)
        {
            bool dodac = true;
            string[] Osrodek = new string[7];

            Osrodek[0] = textBox24.Text.Trim();
            Osrodek[1] = textBox23.Text.Trim();
            Osrodek[2] = textBox34.Text.Trim();
            Osrodek[3] = textBox22.Text.Trim();
            Osrodek[4] = textBox25.Text.Trim();
            Osrodek[5] = textBox32.Text.Trim();
            Osrodek[6] = textBox33.Text.Trim();
            for (int i = 0; i < 7; i++)
            {
                if (Osrodek[i] == "") dodac = false;
            }
            if (dodac) AddOsrodek(Osrodek);
            else MessageBox.Show("Muszą być wypełnione wszystkie pola,\naby dodać/zmienić ośrodek");
        }
        // usuwanie osrodka
        private void button18_Click(object sender, EventArgs e)
        {
            UsunOsrodek( textBox24.Text.Trim() );
            textBox24.Clear();
            textBox23.Clear();
            textBox34.Clear();
            textBox22.Clear();
            textBox25.Clear();
            textBox32.Clear();
            textBox33.Clear();
            button18.Enabled = false;
        }
        // czyszczenie  -> po wcisnieciu button <czysc> danych zwiazanych z osrodkiem
        private void button17_Click(object sender, EventArgs e)
        {

            textBox24.Clear();
            textBox23.Clear();
            textBox34.Clear();
            textBox22.Clear();
            textBox25.Clear();
            textBox32.Clear();
            textBox33.Clear();
            button18.Enabled = false;

        }




        //////////////////////////////////       ZMIANY i EVENTY DOTYCZACE ZAKŁADKI Wychowawcy

        // rozdziela turnusy i zleceniodawcow 

        private List<char[]> Unpack( string data)
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
                char[] unp ={'E','m','p','t','y'};
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
                        temp.Add( Convert.ToInt32(str) );
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
        private void FillTreeWychowawcy( string connStr)
        {

            SqlDataReader myReader;
            SqlCommand myCommand;
            //string fill;
            int idWych = -1;
            string[] data = new string[9];
            for (int ii = 0; ii < 9; ii++)
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

            //fill = "SELECT Turnus, Zleceniodawca, Kto, Telefon, Kod_pocz, Ul, Nr_lokalu, Poczta, Uwagi FROM WYCHOWAWCY";

            try
            {
                myCommand = new SqlCommand(connStr, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    idWych = (int)myReader["Id"];
                    data[0] = (string)myReader["Turnus"];      // wychowawca
                    data[1] = (string)myReader["Zleceniodawca"];
                    data[2] = (string)myReader["Kto"];
                    data[3] = (string)myReader["Telefon"];
                    data[4] = (string)myReader["Kod_pocz"];      // wychowawca
                    data[5] = (string)myReader["Ul"];
                    data[6] = (string)myReader["Nr_lokalu"];
                    data[7] = (string)myReader["Poczta"];
                    if (myReader["Uwagi"] != DBNull.Value)
                    {
                        data[8] = (string)myReader["Uwagi"];
                    }

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
                    for (int i = 0; i < 9; i++)
                    {
                        if (data[i] == "") data[i] = "-";
                    }
                    //data[8] = "lolek";
                    if (tempTurnus.Count == 0) tempTurnus.Add(unp);
                    if (tempZlec2.Count == 0) tempZlec2.Add(unp);
                    if (tempzlecTurnus.Count == 0) tempzlecTurnus.Add(-1);

                    tempWych = new TreeWychowawcy(tempzlecTurnus, tempTurnus, tempZlec2, data[2], data[3], data[4], data[5], data[6], data[7], data[8], idWych);
                    idWych = -1;
                    treeWychowawcy.Add(tempWych);

                }
                myReader.Close();
            }
            catch (Exception blad7)
            {
                MessageBox.Show(blad7.ToString());
            }

        }
        // zlicza ilosc dzieci na wybranym turnusie
        private void IleDzieci()
        {

            //SqlDataReader myReader;
            //SqlCommand myCommand;
            //string dataStreem;
            //string fill;
            //int count_turnus = 0;

            // zlicza ilosc dzieci na turnusie
            /*
            if ( (
                comboBox4.Text != "" ) && ( comboBox4.SelectedIndex < tab3ComboBox4.Count ) )
            {
                
                fill = "SELECT Il_dzieci FROM GLOWNA WHERE Turnus ='" + tab3ComboBox4[comboBox4.SelectedIndex].ToString() + "'";
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

                label39.Text = "" + count_turnus.ToString();
            }


            if ((comboBox4.Text != "") && (comboBox4.SelectedIndex < tab3ComboBox4.Count))
            {

                count_turnus = 0;
                fill = "SELECT Il_dzieci FROM TURNUSY WHERE Turnus ='" + tab3ComboBox4[comboBox4.SelectedIndex].ToString() + "'";
                dataStreem = "Il_dzieci";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        count_turnus = (int)myReader[dataStreem];
                    }
                    myReader.Close();
                }
                catch (Exception blad7)
                {
                    MessageBox.Show(blad7.ToString());
                }

                label45.Text = "" + count_turnus.ToString();
            }
             */

        }
        // zlicza liczbe wychowawcow na wybranym turnusie
        private void IleWychowawcow()
        {
            //zmienne wspolne
            //SqlDataReader myReader;
            //SqlCommand myCommand;
            //string fill;
            //int count = 0;
            /*                     
            if ((comboBox4.Text != "") && (comboBox4.SelectedIndex < tab3ComboBox4.Count) && (comboBox4.SelectedIndex >= 0))
            {
                fill = "SELECT il_wych FROM TURNUSY WHERE Turnus ='" + tab3ComboBox4[comboBox4.SelectedIndex].ToString() + "'";
                try
                {
                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        count = (int)myReader["il_wych"];
                    }
                    myReader.Close();
                }
                catch (Exception blad4)
                {
                    MessageBox.Show(blad4.ToString());
                }

                label40.Text = count.ToString();
            }
             */
        }
        // wypelnia danymi combobox turnusow
        private void FillUpCheckedListBox1()
        {
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string[] dataStreem = new string[3];
            
            //int selIdx = comboBox4.SelectedIndex;
            dataStreem[0] = "";
            checkedListBox1.Items.Clear();
            tab3CheckedListBox1.Clear();

            fill = "SELECT Turnus, Od, Do FROM TURNUSY";

            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    dataStreem[0] = (string)myReader["Turnus"];      // wychowawca
                    dataStreem[1] = (string)myReader["Od"];
                    dataStreem[2] = (string)myReader["Do"];
                    checkedListBox1.Items.Add( ( dataStreem[0].Trim() + "               " + dataStreem[1].Trim() + " - " + dataStreem[2].Trim() ) );
                    tab3CheckedListBox1.Add( dataStreem[0] );
                }
                myReader.Close();
            }
            catch (Exception blad7)
            {
                MessageBox.Show(blad7.ToString());
            }

            //tab3CheckedListBox1.Sort();  //sortuje alfabetycznie tablice ( bo posortowana jest rowniez ta w comboBox4)
            //if (comboBox4.Items.Count > selIdx) comboBox4.SelectedIndex = selIdx;
        }
        //przeszukuje czy da dany turnus jest na liscie turnusow danego wychowawcy
        private bool searchTurnus(TreeWychowawcy p)
        {
            ///MessageBox.Show(tab3CheckedListBox1[currentWybTur].Trim());
            return p.containsTurnus( tab3CheckedListBox1[currentWybTur].Trim() );
        }
        // sprawdza czy jest dany wychowawca na liscie treewychowawcy
        private bool searchKto(TreeWychowawcy p)
        {
            return p.containsKto(listView2.SelectedItems[0].SubItems[0].Text);
        }
        // powoduje odpowiednie ustawienie danych w oknie po zmiane turnusa w combobox4
        /*
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab3ComboBox4 == null)
            {
                tab3ComboBox4 = new List<string>();
            }

            FillTreeWychowawcy("SELECT Id, Turnus, Zleceniodawca, Kto, Telefon, Kod_pocz, Ul, Nr_lokalu, Poczta, Uwagi FROM WYCHOWAWCY");
            FillListView2();
            IleDzieci();
            IleWychowawcow();
        }
         */
        // ustawia wszystkie dane po zmianie wybranego wychowawcy z danego turnusa (listview2)
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string[] dataStreem = new string[5];
            for (int tt = 0; tt < 5; tt++)
            {
                dataStreem[tt] = "";
            }
                if (listView2.SelectedItems.Count > 0)
                {

                    fill = "SELECT Kod_pocz, Ul, Nr_lokalu, Poczta, Uwagi FROM WYCHOWAWCY WHERE Kto ='" + listView2.SelectedItems[0].SubItems[1].Text.Trim() + "'";

                    try
                    {
                        myCommand = new SqlCommand(fill, myCon);
                        myReader = myCommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            dataStreem[0] = (string)myReader["Ul"];      // wychowawca
                            dataStreem[1] = (string)myReader["Nr_lokalu"];
                            dataStreem[2] = (string)myReader["Kod_pocz"];
                            dataStreem[3] = (string)myReader["Poczta"];
                            if (myReader["Uwagi"] != DBNull.Value)
                            {
                                dataStreem[4] = (string)myReader["Uwagi"];
                            }
                        }
                        myReader.Close();
                    }
                    catch (Exception blad7)
                    {
                        MessageBox.Show(blad7.ToString());
                    }

                    if ((tabWychListWychFocus != -1) && (listView2.Items.Count > tabWychListWychFocus))
                    {
                        listView2.Items[tabWychListWychFocus].ForeColor = Color.Black;
                    }
                    tabWychListWychFocus = listView2.SelectedItems[0].Index;
                    listView2.Items[tabWychListWychFocus].ForeColor = Color.Blue;
                }

        }
        // wypelnia danymi liste wychowawcow zapisanych na wybrany turnus
        private void FillListView2()
        {

            //string fill;
            string data;
            int count = 1;
            int index = 1;

            listView2.Items.Clear();
            
            if (wybTurnusy.Count > 0)
            {
                count = 0;
                foreach (int k in wybTurnusy)
                {
                    count = 0;
                    currentWybTur = k;
                    while (count != -1)
                    {
                        count = treeWychowawcy.FindIndex(count, searchTurnus);
                        if (count != -1)
                        {
                            data = treeWychowawcy[count].Kto();

                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = index.ToString();
                            lvi.SubItems.Add(treeWychowawcy[count].Kto());
                            lvi.SubItems.Add(treeWychowawcy[count].Telefon());
                            lvi.SubItems.Add(treeWychowawcy[count].Id().ToString());
                            lvi.SubItems.Add(treeWychowawcy[count].Ul());
                            lvi.SubItems.Add(treeWychowawcy[count].Kod_pocz());
                            lvi.SubItems.Add(treeWychowawcy[count].Poczta());
                            lvi.SubItems.Add(treeWychowawcy[count].Uwagi());
                            listView2.Items.Add(lvi);

                            index++;
                            count++;
                        }
                    }
                }
                
            }
            else
            {
                count = 1;
                for (int i = 0; i < treeWychowawcy.Count; i++ )
                {
                        //data = treeWychowawcy[i].Kto();

                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = count.ToString();
                        lvi.SubItems.Add(treeWychowawcy[i].Kto());
                        lvi.SubItems.Add(treeWychowawcy[i].Telefon());
                        lvi.SubItems.Add(treeWychowawcy[i].Id().ToString() );
                        lvi.SubItems.Add(treeWychowawcy[i].Ul());
                        lvi.SubItems.Add(treeWychowawcy[i].Kod_pocz());
                        lvi.SubItems.Add(treeWychowawcy[i].Poczta());
                        lvi.SubItems.Add(treeWychowawcy[i].Uwagi());
                        listView2.Items.Add(lvi);
                        count++;
                }
            }

            
        }
        // ustawia dane po wejsciu na zakladke "wychowawcy"
        private void tabPage5_Enter(object sender, EventArgs e)
        {
            if (tab3CheckedListBox1 == null)
            {
                tab3CheckedListBox1 = new List<string>();
            }

           
            FillTreeWychowawcy("SELECT Id, Turnus, Zleceniodawca, Kto, Telefon, Kod_pocz, Ul, Nr_lokalu, Poczta, Uwagi FROM WYCHOWAWCY");
            FillUpCheckedListBox1();
            FillListView2();
            
            label39.Text = ""; // ilosc wych na turnusie
            label40.Text = ""; // docelowa ilosc dzieci na turnusie
            label45.Text = ""; // aktualna ilosc dzieci na turnusie
        }

        private void tabPage5_Leave(object sender, EventArgs e)
        {
            //czy do czegos potrzebne ?
        }
        //wyszukiwanie wychowawcy
        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)  // 13 is the ASCII value for a carriage return
            {
                string fill;
                
                treeWychowawcy.Clear();
                listView2.Items.Clear();

                fill = "SELECT Id, Turnus, Zleceniodawca, Kto, Telefon, Kod_pocz, Ul, Nr_lokalu, Poczta, Uwagi FROM WYCHOWAWCY WHERE Kto LIKE '%" + textBox21.Text + "%'";
                FillTreeWychowawcy(fill);

                FillListView2();

                label39.Text = "0";
                label40.Text = "0";
                label45.Text = "0";

                textBox21.Text = ""; //czysci wyszukiwanie
            }
        }
        private void checkedListBox1_ItemCheck_1(object sender, ItemCheckEventArgs e)
        {
            //string fill;
            //string tmp = "";
            bool isChecked = false;
            
            wybTurnusy.Clear();

            foreach (int indexChecked in checkedListBox1.CheckedIndices)
            {
                if (indexChecked != checkedListBox1.SelectedIndex)
                    wybTurnusy.Add(indexChecked);
                else
                {
                    wybTurnusy.Remove(checkedListBox1.SelectedIndex);
                    isChecked = true;
                }
            }
            if (!isChecked) wybTurnusy.Add(checkedListBox1.SelectedIndex);

            FillListView2();
        }
        // button usun
        private void button11_Click(object sender, EventArgs e)
        {
            SqlCommand myCommand;
            string fill;
            bool err = false;

            if ( listView2.SelectedItems.Count > 0 )
                if (IstniejeWychowawca(listView2.SelectedItems[0].SubItems[1].Text))
                {
                string message = "Usunąc : " + listView2.SelectedItems[0].SubItems[1].Text + " z bazy?";
                string caption = "Ostrzeżenia";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(this, message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    int idWych =-1;
                    for (int i = 0; i < listView2.Items.Count; i++)
                    {
                        if (listView2.Items[i].ForeColor == Color.Blue)
                        {
                            MessageBox.Show(listView2.Items[i].SubItems[3].Text);
                            idWych = Convert.ToInt16(listView2.Items[i].SubItems[3].Text);
                        }
                    }
                    fill = "DELETE FROM WYCHOWAWCY WHERE Id =" + idWych;
                    try
                    {
                        myCommand = new SqlCommand(fill, myCon);
                        myCommand.ExecuteNonQuery();
                    }
                    catch (Exception blad4)
                    {
                        err = true;
                        fill = blad4.ToString();
                    }
                    if (err) MessageBox.Show("Wystąpił problem z usunięciem rekordu\n" + fill);
                    else
                    {
                        MessageBox.Show("Usunięto poprawnie rekord");
                        if (tab3CheckedListBox1 == null)
                        {
                            tab3CheckedListBox1 = new List<string>();
                        }
                        FillTreeWychowawcy("SELECT Id, Turnus, Zleceniodawca, Kto, Telefon, Kod_pocz, Ul, Nr_lokalu, Poczta, Uwagi FROM WYCHOWAWCY");
                        FillListView2();
                        IleDzieci();
                        IleWychowawcow();
                    }
                }
                }
            else
            {
                MessageBox.Show("Nie znaleziono :" + listView2.SelectedItems[0].SubItems[1].Text + " w bazie wychowawców\nZostał już usunięty ?");
            }
        }
        //button drukuj
        private void button13_Click(object sender, EventArgs e)
        {

        }
        //button dodaj
        private void button8_Click(object sender, EventArgs e)
        {
            Form8 dlg = new Form8(myCon ,-1);
            dlg.Show();
        }
        //podwojne klikniecie na listview2 :P
        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                Form8 dlg = new Form8(myCon, 2);
                dlg.Show();
            }

        }



        //////////////////////////////////       ZMIANY i EVENTY DOTYCZACE ZAKŁADKI Zlecenia

        //dodaj nowe zlecenie
        private void SetAlternatingRowColors(ListView lst, Color color1, Color color2)
        {
            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            bool isStow = false;
            string fill;

            //loop through each ListViewItem in the ListView control
            foreach (ListViewItem item in lst.Items)
            {
                isStow = false;
                fill = "SELECT Stow FROM GLOWNA WHERE Zleceniodawca ='" + item.SubItems[1].Text + "'";
                //MessageBox.Show("Listview:\n" + item.SubItems[1].Text);
                try
                {

                    myCommand = new SqlCommand(fill, myCon);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        isStow = (bool)myReader["Stow"];
                        //data = (string)myReader["Turnus"];
                        //MessageBox.Show("isStow = " + isStow.ToString() );
                    }
                    myReader.Close();
                }
                catch (Exception blad4)
                {
                    MessageBox.Show(blad4.ToString());
                }
                
                if (isStow)
                    item.ForeColor = color1;
                else
                    item.ForeColor = color2;
                 
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            Form4 dlg = new Form4(myCon);
            DialogResult arg;
            arg = dlg.ShowDialog();
            FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA");
            if(listView7.Items.Count > 0 )listView7.Items[listView7.Items.Count - 1].EnsureVisible();
        }
        //przejscie na zakladke
        private void tabPage8_Enter(object sender, EventArgs e)
        {

            button12.Text = "Dodaj";
            button14.Text = "Usuń";
            button20.Text = "Filtruj";
            button21.Text = "Cofnij filtr";

            radioButton1.Text = "Zlecenie";
            radioButton2.Text = "Turnus";
            radioButton3.Text = "Wszędzie";

            listView7.Columns.Clear();
            listView7.Columns.Add(new ColHeader("Lp.", 50, HorizontalAlignment.Left, true));
            listView7.Columns.Add(new ColHeader("ID", 50, HorizontalAlignment.Left, true));
            listView7.Columns.Add(new ColHeader("Organizator", 80, HorizontalAlignment.Left, true));
            listView7.Columns.Add(new ColHeader("Zleceniodawca", 200, HorizontalAlignment.Left, true));
            listView7.Columns.Add(new ColHeader("Il. dzieci", 60, HorizontalAlignment.Left, true));
            listView7.Columns.Add(new ColHeader("Turnus", 200, HorizontalAlignment.Left, true));
            listView7.Columns.Add(new ColHeader("Uwagi", 280, HorizontalAlignment.Left, true));
            listView7.Columns.Add(new ColHeader("Wyjazd", 280, HorizontalAlignment.Left, true));
            listView7.Columns.Add(new ColHeader("Powrót", 280, HorizontalAlignment.Left, true));

            if ((myCon != null) && (myCon.State == ConnectionState.Open))
            {
                FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA");
            }

        }

        //wypelnia danymi listview
        private void FillListView7( string textFiltr)
        {

            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string[] dataStreem = new string[15];
            //string dataStreem2;
            bool powiadomiono = false;
            int count = 0;
            
            // Initialize the dialog that will contain the progress bar
            Form14 progressDialog = new Form14();
            // Initialize the thread that will handle the background process
            Thread backgroundThread = new Thread(new ThreadStart(() =>
            {
                // Set the dialog to operate in indeterminate mode
                progressDialog.SetIndeterminate(true);
                progressDialog.ShowDialog();
            }
            ));

            // Start the background process thread
            //this.Enabled = false;
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
            
            listView7.Items.Clear();

            // wybrano oba warunki w turnus i zleceniodawca
            if (textFiltr == "")
            {
                fill = "SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA";
            }
            else
            {
                fill = textFiltr;
            }

            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    count++;
                    dataStreem[0] = Convert.ToString(myReader["Id"]);    // ilosc dzieci
                    dataStreem[1] = Convert.ToString(myReader["Il_dzieci"]);    // ilosc dzieci
                    dataStreem[2] = (string)myReader["Os_kontaktowa"];      // osoba kontaktowa
                    dataStreem[3] = (string)myReader["Tel_kontaktowy"];      // telefon do os. kontaktowej
                    dataStreem[4] = (string)myReader["Ul"];      // telefon do os. kontaktowej
                    dataStreem[5] = (string)myReader["Kod_pocz"];      // telefon do os. kontaktowej
                    dataStreem[6] = (string)myReader["Miejscowosc"];      // telefon do os. kontaktowej
                    dataStreem[7] = (string)myReader["Nr_lokalu"];      // telefon do os. kontaktowej
                    powiadomiono = (bool)myReader["Powiadomiono"];
                    if (powiadomiono)
                    {
                        dataStreem[8] = (string)myReader["Data_powiadom"];      // telefon do os. kontaktowej
                    }
                    else
                    {
                        dataStreem[8] = "nie powiadomiono";
                    }
                    if (myReader["Wyjazd"] != DBNull.Value)
                    {
                        dataStreem[9] = (string)myReader["Wyjazd"];
                        if (dataStreem[9].Length > 50)
                        {
                            dataStreem[9] = dataStreem[9].Remove(50);
                            dataStreem[9] += "...";
                        }
                    }
                    else
                    {
                        dataStreem[9] = "";
                    }
                    if (myReader["Powrot"] != DBNull.Value)
                    {
                        dataStreem[13] = (string)myReader["Powrot"];
                        if (dataStreem[13].Length > 50)
                        {
                            dataStreem[13] = dataStreem[13].Remove(50);
                            dataStreem[13] += "...";
                        }
                    }
                    else
                    {
                        dataStreem[13] = "";
                    }
                    if (myReader["Organizator"] != DBNull.Value)
                    {
                        dataStreem[14] = (string)myReader["Organizator"];
                    }
                    else
                    {
                        dataStreem[14] = "";
                    }
                    dataStreem[10] = (string)myReader["Uwagi"];
                    dataStreem[10] = dataStreem[9].Replace("\n"," ");
                    dataStreem[11] = (string)myReader["Zleceniodawca"];
                    dataStreem[12] = (string)myReader["Turnus"];
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = count.ToString();
                    lvi.SubItems.Add( dataStreem[0]);
                    lvi.SubItems.Add(dataStreem[14]);
                    lvi.SubItems.Add( dataStreem[11]);
                    lvi.SubItems.Add( dataStreem[1]);
                    lvi.SubItems.Add( dataStreem[12]);
                    lvi.SubItems.Add( dataStreem[10]);
                    lvi.SubItems.Add( dataStreem[9]);
                    lvi.SubItems.Add( dataStreem[13]);
                    listView7.Items.Add(lvi);
                }
                myReader.Close();
            }
            catch (Exception blad5)
            {
                MessageBox.Show(blad5.ToString());
            }
            if (isSavedListView7)
            {
                listView7_sort();
            }
            if (listView7.Items.Count >= saveSelectedItem && listView7.Items.Count > 0)
            {
                listView7.FocusedItem = listView7.Items[saveSelectedItem];
                listView7.FocusedItem.Selected = true;
                listView7.FocusedItem.EnsureVisible();
                //listView7.Items[saveSelectedItem].EnsureVisible();
            }
            //backgroundThread.Abort();
            
            if (progressDialog.InvokeRequired)        
                progressDialog.BeginInvoke(new Action(() => progressDialog.Close()));
            
            //Application.DoEvents();
            //this.Enabled = true;
            //this.TopMost = true;
            //Thread.Sleep(100);
            listView7.Focus();
            //this.TopMost = false;
        } // END private void FillListView7()

        //sortuje zaleznie od kliknietej kolumny
        private void listView7_ColumnClick(object sender, ColumnClickEventArgs e)
        {     
            // Create an instance of the ColHeader class.
            ColHeader clickedCol = (ColHeader)this.listView7.Columns[e.Column];
            // Set the ascending property to sort in the opposite order.
            clickedCol.ascending = !clickedCol.ascending;
            // Get the number of items in the list.
            int numItems = this.listView7.Items.Count;
            //save all info to auto set it back
            saved_clickedCol = clickedCol;
            saved_ascending = clickedCol.ascending;
            saved_column_id = e.Column;
            isSavedListView7 = true;

            // Turn off display while data is repopulated.
            this.listView7.BeginUpdate();

            // Populate an ArrayList with a SortWrapper of each list item.
            ArrayList SortArray = new ArrayList();
            for (int i = 0; i < numItems; i++)
            {
                SortArray.Add(new SortWrapper(this.listView7.Items[i], e.Column));
            }

            // Sort the elements in the ArrayList using a new instance of the SortComparer
            // class. The parameters are the starting index, the length of the range to sort,
            // and the IComparer implementation to use for comparing elements. Note that
            // the IComparer implementation (SortComparer) requires the sort
            // direction for its constructor; true if ascending, othwise false.
            SortArray.Sort(0, SortArray.Count, new SortWrapper.SortComparer(clickedCol.ascending));

            // Clear the list, and repopulate with the sorted items.
            this.listView7.Items.Clear();
            for (int i = 0; i < numItems; i++)
                this.listView7.Items.Add(((SortWrapper)SortArray[i]).sortItem);

            // Turn display back on.
            this.listView7.EndUpdate();
  
        }

        //sortuje zaleznie od kliknietej kolumny
        private void listView7_sort()
        {

            // Create an instance of the ColHeader class.
            ColHeader clickedCol = saved_clickedCol;
            //saved_clickedCol = clickedCol;

            // Set the ascending property to sort in the opposite order.
            clickedCol.ascending = saved_ascending;// !clickedCol.ascending;
            //saved_ascending = clickedCol.ascending;

            // Get the number of items in the list.
            int numItems = this.listView7.Items.Count;

            // Turn off display while data is repopulated.
            this.listView7.BeginUpdate();

            // Populate an ArrayList with a SortWrapper of each list item.
            ArrayList SortArray = new ArrayList();
            for (int i = 0; i < numItems; i++)
            {
                SortArray.Add(new SortWrapper(this.listView7.Items[i], saved_column_id));
            }

            // Sort the elements in the ArrayList using a new instance of the SortComparer
            // class. The parameters are the starting index, the length of the range to sort,
            // and the IComparer implementation to use for comparing elements. Note that
            // the IComparer implementation (SortComparer) requires the sort
            // direction for its constructor; true if ascending, othwise false.
            SortArray.Sort(0, SortArray.Count, new SortWrapper.SortComparer(clickedCol.ascending));

            // Clear the list, and repopulate with the sorted items.
            this.listView7.Items.Clear();
            for (int i = 0; i < numItems; i++)
                this.listView7.Items.Add(((SortWrapper)SortArray[i]).sortItem);

            // Turn display back on.
            this.listView7.EndUpdate();

        }

        //wyswietla szczegoly dwa razy kliknietego rekordu
        private void listView7_DoubleClick(object sender, EventArgs e)
        {
            if (listView7.SelectedItems.Count > 0)
            {
                saveSelectedItem = listView7.SelectedIndices[0];

                Form7 dlg = new Form7(myCon,listView7.SelectedItems[0].SubItems[3].Text, listView7.SelectedItems[0].SubItems[5].Text, toolStripStatusLabel1.Text);
                DialogResult arg;
                arg = dlg.ShowDialog();
                FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA");
                //if (listView7.Items.Count > 0) listView7.Items[listView7.Items.Count - 1].EnsureVisible();
                //FillListView7("SELECT Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom FROM GLOWNA");
            }

        }
        // usuwa zlecenie
        private void button14_Click(object sender, EventArgs e)
        {
            if (listView7.SelectedItems.Count > 0)
            {
                string message = "Uwaga rekord zostanie usunięty\n " + listView7.SelectedItems[0].SubItems[1].Text + "\n" + listView7.SelectedItems[0].SubItems[3].Text;
                string caption = "Ostrzeżenie";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(this, message, caption, buttons);
                switch (result)
                {
                    case DialogResult.No:
                        break;

                    case DialogResult.Yes:
                        if (IstniejeZleceniodawca(listView7.SelectedItems[0].SubItems[3].Text, listView7.SelectedItems[0].SubItems[5].Text))
                        {
                            ClearWych(listView7.SelectedItems[0].SubItems[3].Text.Trim(), listView7.SelectedItems[0].SubItems[5].Text.Trim());//usuwa w wychowawcach wpisy o takim zleceniodawcuy jezeli ktos byl na niego wpisany
                            //zmienne wspolne
                            SqlCommand myCommand;
                            string fill;
                            bool err = false;

                            fill = "DELETE FROM GLOWNA WHERE Zleceniodawca ='" + listView7.SelectedItems[0].SubItems[3].Text + "' AND Turnus ='" + listView7.SelectedItems[0].SubItems[5].Text + "'";
                            try
                            {
                                myCommand = new SqlCommand(fill, myCon);
                                myCommand.ExecuteNonQuery();
                            }
                            catch (Exception blad4)
                            {
                                err = true;
                                fill = blad4.ToString();
                            }
                            if (err) MessageBox.Show("Wystąpił problem z usunięciem rekordu\n" + fill);
                            else MessageBox.Show("Usunięto poprawnie zleceniodawce");
                        }
                        else MessageBox.Show("Nie ma w bazie takiego zleceniodawcy ^^");
                        FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA");
                        break;
                }
                //MessageBox.Show("Czy zapisać zmiany ??");

            }
        }
        //filtruje liste
        private void button20_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA WHERE Zleceniodawca LIKE '%" + textBox36.Text + "%'");
                //MessageBox.Show("SELECT Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Transport, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom FROM GLOWNA WHERE Zleceniodawca LIKE '%" + textBox36.Text + "%'");
            }
            else
                if (radioButton2.Checked)
                {
                    FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA WHERE Turnus LIKE '%" + textBox36.Text + "%'");
                }
                else
                    if (radioButton3.Checked)
                    {
                        FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA WHERE Turnus LIKE '%" + textBox36.Text + "%' OR Zleceniodawca LIKE '%" + textBox36.Text + "%' OR Miejscowosc LIKE '%" + textBox36.Text + "%' OR Kod_pocz LIKE '%" + textBox36.Text + "%' OR Os_kontaktowa LIKE '%" + textBox36.Text + "%' OR Ul LIKE '%" + textBox36.Text + "%'");
                    }

        }
        // fultruje jezeli wcisnieto enter w textboxsie
        private void textBox36_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)  // 13 is the ASCII value for a carriage return
            {

                if (radioButton1.Checked)
                {
                    FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA WHERE Zleceniodawca LIKE '%" + textBox36.Text + "%'");
                    //MessageBox.Show("SELECT Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Transport, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom FROM GLOWNA WHERE Zleceniodawca LIKE '%" + textBox36.Text + "%'");
                }
                else
                    if (radioButton2.Checked)
                    {
                        FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA WHERE Turnus LIKE '%" + textBox36.Text + "%'");
                    }
                    else
                        if (radioButton3.Checked)
                        {
                            FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA WHERE Turnus LIKE '%" + textBox36.Text + "%' OR Zleceniodawca LIKE '%" + textBox36.Text + "%' OR Miejscowosc LIKE '%" + textBox36.Text + "%' OR Kod_pocz LIKE '%" + textBox36.Text + "%' OR Os_kontaktowa LIKE '%" + textBox36.Text + "%' OR Ul LIKE '%" + textBox36.Text + "%'");
                        }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            FillListView7("SELECT Id, Turnus, Zleceniodawca, Il_dzieci, Os_kontaktowa, Tel_kontaktowy, Od, Do, Ul, Kod_pocz, Miejscowosc, Nr_lokalu, Id_powiadom, Wyjazd, Powrot, Uwagi, Powiadomiono, Id_powiadom, Data_powiadom, Organizator FROM GLOWNA");
        }




        // zmienia wyjaz/powort dla wszystkich wyswietlajac i czkajac na potwierdzenie -> menu na gorze "Update transport"
        private void updateTransportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form10 dlg = new Form10(myCon);
            dlg.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(lastTick)
            {
                tabPage10.ForeColor = Color.Green;
                lastTick = false;
            }
                else
            {
                tabPage10.ForeColor = Color.Red;
                lastTick = true;
            }
        }

        private void nowaBazaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form12 dlg = new Form12();
            dlg.ShowDialog();
        }

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage8_Leave(object sender, EventArgs e)
        {
            if (listView7.Items.Count > 0)
            {
                saveSelectedItem = listView7.SelectedIndices[0];
                //saveSelectedItem = listView7.TopItem.Index;
            }
        }

        private void deleteBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction...");
        }

        private void powtórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction...");
        }

        private void cofnijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction...");
        }

        private void exportujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction...");
        }

        private void importujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction...");
        }

        private void opcjeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Under construction...");
        }
    }
}
