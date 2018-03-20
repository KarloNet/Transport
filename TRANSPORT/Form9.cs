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
    public partial class Form9 : Form
    {

        //public event ValueUpdatedEventHandler ValueUpdated;

        List<Turnusy> listTurnusy;

        public Form9( List<Turnusy>listTurnusyOld )
        {
            listTurnusy = new List<Turnusy>();

            for (int i = 0; i < listTurnusyOld.Count; i++)
            {
                listTurnusy.Add(listTurnusyOld[i]);
            }

            InitializeComponent();
        }

        private void FillUp()
        {

            for (int i = 0; i < listTurnusy.Count; i++)
            {
                 checkedListBox1.Items.Add(listTurnusy[i].GetTurnus() + "\t\t" + listTurnusy[i].GetOd() + " - " + listTurnusy[i].GetDo());
            }

        }

        private void Form9_Load(object sender, EventArgs e)
        {
            if (listTurnusy == null)
            {
                listTurnusy = new List<Turnusy>();
            }
            else
            {
                ;//listTurnusy.Clear();
            }
            button1.Text = "Ok";
            button2.Text = "Anuluj";

            FillUp();

        }

        private void Form9_FormClosing(object sender, FormClosingEventArgs e)
        {
            
                
        }

        /*
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string newValue = this.textBox1.Text;
            Dane tmp = new Dane();
            tmp.stringVal = newValue;
            tmp.val = 9;
            ValueUpdatedEventArgs valueArgs = new ValueUpdatedEventArgs(tmp);
            ValueUpdated(this, valueArgs);
        }
        */

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }


    }
}
