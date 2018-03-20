using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace TRANSPORT
{
    public partial class Form10 : Form
    {
        SqlConnection myConn;
        List<int> idList;
        int index = 0;

        public Form10(SqlConnection connection)
        {
            myConn = connection;
            InitializeComponent();
        }

        private void FillUp( int id)
        {

            //zmienne wspolne
            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            string dataStreem = "";
            string dataStreem2 = "";
            string dataStreem3 = "";
            char tab = '\u0009';

            // wybrano oba warunki w turnus i zleceniodawca
            fill = "SELECT Wyjazd, Powrot FROM GLOWNA WHERE Id =" + id;
            try
            {
                myCommand = new SqlCommand(fill, myConn);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    textBox1.Clear();
                    textBox2.Clear();
                    if (myReader["Wyjazd"] != DBNull.Value)
                    {
                        dataStreem = (string)myReader["Wyjazd"];
                        dataStreem = dataStreem.Replace(tab.ToString(), "");
                        dataStreem2 = dataStreem;
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
                        //textBox1.Text = dataStreem;
                        dataStreem3 = dataStreem;
                        //textBox1.Text = (string)myReader["Wyjazd"];
                    }
                    if (myReader["Powrot"] != DBNull.Value)
                    {
                        dataStreem = (string)myReader["Powrot"];
                        dataStreem = dataStreem.Replace(tab.ToString(), "");
                        dataStreem2 = dataStreem2 + "\n POWRÓT: \n" + dataStreem;
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
                        dataStreem3 = dataStreem3 + "\n\n\n\n\n\n\n" + "POWRÓT:" + dataStreem; 
                    }
                    textBox1.Text = dataStreem2;
                    textBox2.Text = dataStreem3;
                }

                myReader.Close();
                label1.Text = idList[index].ToString();
                label2.Text = index.ToString();
            }
            catch (Exception blad5)
            {
                MessageBox.Show(blad5.ToString());
            }

        }

        private void Form10_Load(object sender, EventArgs e)
        {

            button1.Text = "Next";
            button2.Text = "Prev";
            button3.Text = "Save";
            idList = new List<int>();

            SqlDataReader myReader;
            SqlCommand myCommand;
            string fill;
            int iD = 0;
            fill = "SELECT Id FROM GLOWNA";
            try
            {
                myCommand = new SqlCommand(fill, myConn);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    iD = (int)myReader["Id"];
                    idList.Add(iD);
                }
                myReader.Close();
            }
            catch (Exception blad5)
            {
                MessageBox.Show(blad5.ToString());
            }

           if(idList.Count > 0) FillUp(idList[index]);
           label1.Text = idList[index].ToString();
           label2.Text = index.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            index++;
            if (index > idList.Count) index = idList.Count;
            if( index <= idList.Count) FillUp(idList[index]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            index--;
            if (index < 0) index = 0;
            if (idList.Count > 0) FillUp(idList[index]);
        }

    }
}
