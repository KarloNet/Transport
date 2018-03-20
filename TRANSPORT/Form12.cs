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
    public partial class Form12 : Form
    {

        private string id = null;
        private string pw = null;
        private string server = null;
        private string trusted = null;
        private string database = null;
        private string timeout = null;

        public Form12()
        {
            InitializeComponent();
            ReadIni();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                button1.Enabled = false;
                button2.Enabled = false;
                CreateDatabase();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void ReadIni()
        {
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
                id = Program.XorString(read.ReadLine(), Program.XOR_KEY);
                pw = Program.XorString(read.ReadLine(), Program.XOR_KEY);
                server = Program.XorString(read.ReadLine(), Program.XOR_KEY);
                trusted = Program.XorString(read.ReadLine(), Program.XOR_KEY);
                database = Program.XorString(read.ReadLine(), Program.XOR_KEY);
                timeout = Program.XorString(read.ReadLine(), Program.XOR_KEY);
                read.Close();
                if ((id == null) || (pw == null) || (server == null) || (trusted == null) || (database == null) || (timeout == null))
                {
                    MessageBox.Show("Błąd w pliku system.ini\n");
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Nie udało się otowrzyć pliku system.ini\n");
                Close();
            }
             // wczytaj bazy
            //MessageBox.Show("Wybrano BASE : " + curentBase + "\n");
        }

        private void AddBaseColumns(string conn)
        {
            SqlConnection myCon = null;
            SqlCommand myCommand;
            textBox2.Text += "Connecting to new created database....\r\n";
            try
            {
                myCon = new SqlConnection(conn);
                myCon.Open();
            }
            catch (Exception blad)
            {
                textBox2.Text += "Can't connect to new database\r\n";
                textBox2.Text += blad.ToString();
                button2.Enabled = true;
                return;
            }
            textBox2.Text += "Succesfull connected to new database\r\nCreating TABLE GLOWNA ... \r\n";
            try
            {
                myCommand = new SqlCommand(@"CREATE TABLE GLOWNA (
                Id int IDENTITY PRIMARY KEY NOT NULL,
                Turnus varchar(MAX) DEFAULT '<brak>',
                Od varchar(10) DEFAULT '...',
                Do varchar(10) DEFAULT '...',
                Zleceniodawca varchar(MAX) DEFAULT '<brak>',
                Kod_pocz varchar(6) DEFAULT '<brak>',
                Miejscowosc varchar(MAX) DEFAULT '<brak>',
                Ul varchar(MAX) DEFAULT '<brak>',
                Nr_lokalu varchar(10) DEFAULT '<brak>',
                Os_kontaktowa varchar(MAX) DEFAULT '<brak>',
                Tel_kontaktowy varchar(20) DEFAULT '<brak>',
                Il_dzieci int DEFAULT 0,
                Powiadomiono bit DEFAULT 0,
                Id_powiadom int DEFAULT 0,
                Uwagi text DEFAULT ' ',
                Data_powiadom varchar(10),
                Wyjazd varchar(MAX),
                Powrot varchar(MAX),
                Stow bit NOT NULL,
                Organizator varchar(50))", myCon);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception blad2)
            {
                textBox2.Text += "Error creating TABLE GLOWNA\r\n";
                textBox2.Text += blad2.ToString();
                button2.Enabled = true;
                if (myCon.State == ConnectionState.Open)
                {
                    textBox2.Text += "Closing connection....\r\n";
                    myCon.Close();
                }
                return;
            }
            textBox2.Text += "OK\r\n";
            textBox2.Text += "Creating TABLE TURNUSY ... \r\n";
            try
            {
                myCommand = new SqlCommand(@"CREATE TABLE TURNUSY (
                Id int IDENTITY PRIMARY KEY NOT NULL,
                Turnus char(30),
                Od char(10),
                Do char(10),
                Kierownik char(30),
                il_wych int NOT NULL DEFAULT 0,
                il_dzieci int NOT NULL DEFAULT 0 )", myCon);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception blad3)
            {
                textBox2.Text += "Error creating TABLE TURNUSY\r\n";
                textBox2.Text += blad3.ToString();
                button2.Enabled = true;
                if (myCon.State == ConnectionState.Open)
                {
                    textBox2.Text += "Closing connection....\r\n";
                    myCon.Close();
                }
                return;
            }
            textBox2.Text += "OK\r\n";
            textBox2.Text += "Creating TABLE WYCHOWAWCY ... \r\n";
            try
            {
                myCommand = new SqlCommand(@"CREATE TABLE WYCHOWAWCY (
                Id int IDENTITY PRIMARY KEY NOT NULL,
                Turnus varchar(MAX),
                Zleceniodawca varchar(MAX),
                Kto varchar(100) DEFAULT 'Nie wybrano',
                Telefon varchar(20),
                Kod_pocz varchar(10) DEFAULT '<brak>',
                Poczta varchar(100) DEFAULT '<brak>',
                Ul varchar(255) DEFAULT '<brak>',
                Nr_lokalu varchar(20) DEFAULT '<brak>',
                Uwagi text DEFAULT ' ' )", myCon);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception blad5)
            {
                textBox2.Text += "Error creating TABLE WYCHOWAWCY\r\n";
                textBox2.Text += blad5.ToString();
                button2.Enabled = true;
                if (myCon.State == ConnectionState.Open)
                {
                    textBox2.Text += "Closing connection....\r\n";
                    myCon.Close();
                }
                return;
            }
            textBox2.Text += "OK\r\n";
            textBox2.Text += "Creating TABLE OSRODKI ... \r\n";
            try
            {
                myCommand = new SqlCommand(@"CREATE TABLE OSRODKI (
                Id int IDENTITY PRIMARY KEY NOT NULL,
                Osrodek char(100),
                Nazwa char(200),
                Ul char(30),
                Nr_lokalu char(10),
                Kod_pocz char(6),
                Poczta char(30),
                Tel char(20) )", myCon);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception blad7)
            {
                textBox2.Text += "Error creating TABLE OSRODKI\r\n";
                textBox2.Text += blad7.ToString();
                button2.Enabled = true;
                if (myCon.State == ConnectionState.Open)
                {
                    textBox2.Text += "Closing connection....\r\n";
                    myCon.Close();
                }
                return;
            }
            textBox2.Text += "OK\r\n";
/*
            if (fill == "3")
            {
                try
                {
                    myCommand = new SqlCommand("INSERT INTO TURNUSY ( Turnus, Od, Do, Kierownik) VALUES ( 'Biały Dunajec I', '27.06', '05.07','' )", myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad4)
                {
                    Console.WriteLine(blad4.ToString());
                    Console.ReadKey();
                }

            }
           if (fill == "5")
            {
                try
                {
                    myCommand = new SqlCommand("INSERT INTO WYCHOWAWCY (Turnus, Zleceniodawca, Kto, Telefon, Kod_pocz, Poczta, Ul, Nr_lokalu ) VALUES ('Gąsawa I;Gąsawa III;Biały Dunajec III' ,'Kolbuszowa;Łódź' ,'Aneta Skoczylas' ,'600-560-450','23-800','Olsztyn','Polna','33/2') ", myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad6)
                {
                    Console.WriteLine(blad6.ToString());
                    Console.ReadKey();
                }

            }
            if (fill == "7")
            {
                try
                {
                    myCommand = new SqlCommand("INSERT INTO OSRODKI (Osrodek, Nazwa, Ul, Nr_lokalu, Kod_pocz, Poczta, Tel) VALUES ('Gąsawa', 'Z.S. PONADGIMNAZJALNYCH IM. JADWIGI DZIUBIŃSKIEJ','Żnińska','6','88-410','Gąsawa','052 305-50-90') ", myCon);
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception blad6)
                {
                    Console.WriteLine(blad6.ToString());
                    Console.ReadKey();
                }

            }
*/
            if (myCon.State == ConnectionState.Open)
            {
                textBox2.Text += "Closing connection....\r\n";
                myCon.Close();
            }

            textBox2.Text += "Adding new database to list";
            if ((id == null) || (pw == null) || (server == null) || (trusted == null) || (database == null) || (timeout == null))
            {
                textBox2.Text += "Error in file system.ini\nAborting...\r\n";
                button2.Enabled = true;
                return;
            }
            else
                try
                {
                    myCon = new SqlConnection(id + pw + server + trusted + database + timeout);
                    myCon.Open();
                }
                catch (Exception blad8)
                {
                    textBox2.Text += "Can't connect to  database TRANSPORT_INI\r\n";
                    textBox2.Text += blad8.ToString();
                    button2.Enabled = true;
                    return;
                }
            textBox2.Text += "Connected to TRANSPORT_INI\nAdding new created database\r\n";
            
            string fill = "INSERT INTO DBASE VALUES ('" + textBox1.Text + "')";
            try
            {
                myCommand = new SqlCommand(fill, myCon);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception blad9)
            {
                textBox2.Text += "Error adding database\r\n";
                textBox2.Text += blad9.ToString();
                button2.Enabled = true;
                return;
            }
            textBox2.Text += "Database: " + textBox1.Text + " added to TRANSPORT_INI\r\n";
            
            if (myCon.State == ConnectionState.Open)
            {
                textBox2.Text += "Closing connection....\r\n";
                myCon.Close();
            }
            textBox2.Text += "EVERYTHING WENT OK\r\n";

            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void CreateDatabase()
        {

            string str;
            string conn;
            bool isOK = false;
            //MessageBox.Show(id + pw + server + trusted + "database=master;" + timeout);
            string ID = "UID=xx;";
            string PW = "password=XXxxXXxxxxxXXXxxXXxx;";
            conn = ID + PW + server + trusted + "database=master;" + timeout;

            textBox2.Text = "Starting create new DATABASE\n";
            textBox2.Text += "name : " + textBox1.Text + "\r\n";
            textBox2.Text += "Connecting to SQL Server....\r\n";

            SqlConnection myConn = new SqlConnection(conn);
            str = "CREATE DATABASE " + textBox1.Text;
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                textBox2.Text += "Succesfull create database\r\n";
                isOK = true;
            }
            catch (System.Exception ex)
            {
                textBox2.Text += "Can't connect to SQL Server\r\n";
                textBox2.Text += ex.ToString();
                button2.Enabled = true;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    textBox2.Text += "Closing connection....\r\n";
                    myConn.Close();
                }
            }
            if (isOK)//ad permision for standard user
            {
                //System.Threading.Thread.Sleep(2000);
                textBox2.Text += "Adding user roles\r\n";
                myConn = new SqlConnection(conn);
                str = "USE [" + textBox1.Text + "] CREATE USER [TRANSPORT] FOR LOGIN [TRANSPORT] EXEC sp_addrolemember N'db_owner', N'TRANSPORT'";
                myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    textBox2.Text += "Succesfull add permision for standard user\r\n";
                    isOK = true;
                }
                catch (System.Exception ex)
                {
                    textBox2.Text += "Can't connect to SQL Server\r\n";
                    textBox2.Text += ex.ToString();
                    button2.Enabled = true;
                    isOK = false;
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        textBox2.Text += "Closing connection....\r\n";
                        myConn.Close();
                    }
                }
            }
            if (isOK)
            {
                conn = id + pw + server + trusted + "database="+ textBox1.Text +";" + timeout;
                AddBaseColumns(conn);
            }
         }
    }
}
