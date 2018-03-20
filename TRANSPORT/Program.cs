using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

namespace TRANSPORT
{

    /// <summary>
    /// This is a CheckedListBox that allows the item's text color to be different for each of the 3 states of the corresponding checkbox's value.
    /// Like the base CheckedListBox control, you must handle setting of the indeterminate checkbox state yourself.
    /// Note also that this control doesn't allow highlighting of the selected item since that obscures the item's special text color which has the special meaning.  But 
    /// the selected item is still known to the user by the focus rectangle it will have surrounding it, like usual.
    /// </summary>
    class ColorCodedCheckedListBox : CheckedListBox
    {
        public Color UncheckedColor { get; set; }
        public Color CheckedColor { get; set; }
        public Color IndeterminateColor { get; set; }

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ColorCodedCheckedListBox()
        {
            UncheckedColor = Color.Green;
            CheckedColor = Color.Red;
            IndeterminateColor = Color.Orange;
        }

        /// <summary>
        /// Constructor that allows setting of item colors when checkbox has one of 3 states.
        /// </summary>
        /// <param name="uncheckedColor">The text color of the items that are unchecked.</param>
        /// <param name="checkedColor">The text color of the items that are checked.</param>
        /// <param name="indeterminateColor">The text color of the items that are indeterminate.</param>
        public ColorCodedCheckedListBox(Color uncheckedColor, Color checkedColor, Color indeterminateColor)
        {
            UncheckedColor = uncheckedColor;
            CheckedColor = checkedColor;
            IndeterminateColor = indeterminateColor;
        }

        /// <summary>
        /// Overriden draw method that doesn't allow highlighting of the selected item since that obscures the item's text color which has desired meaning.  But the 
        /// selected item is still known to the user by the focus rectangle being displayed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.DesignMode)
            {
                base.OnDrawItem(e);
            }
            else
            {
                Color textColor = this.GetItemCheckState(e.Index) == CheckState.Unchecked ? UncheckedColor : (this.GetItemCheckState(e.Index) == CheckState.Checked ? CheckedColor : IndeterminateColor);

                DrawItemEventArgs e2 = new DrawItemEventArgs
                   (e.Graphics,
                    e.Font,
                    new Rectangle(e.Bounds.Location, e.Bounds.Size),
                    e.Index,
                    (e.State & DrawItemState.Focus) == DrawItemState.Focus ? DrawItemState.Focus : DrawItemState.None, /* Remove 'selected' state so that the base.OnDrawItem doesn't obliterate the work we are doing here. */
                    textColor,
                    this.BackColor);

                base.OnDrawItem(e2);
            }
        }
    }

    public class Dane
    {
        public string stringVal;
        public int val;
    }
    //funkcja sluzaca do obslugi eventhandlera pomiedzy formami przy przekazywaniu danych z formy do formy
    public delegate void ValueUpdatedEventHandler(object sender, ValueUpdatedEventArgs e);
    /// <summary>
    /// Holds the event arguments for the ValueUpdated event.
    /// </summary>
    public class ValueUpdatedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Used to store the new value
        /// </summary>
        private Dane newValue;
        
        /// <summary>
        /// Create a new instance of the ValueUpdatedEventArgs class.
        /// </summary>
        /// <param name="newValue">represents the change to the value</param>
        public ValueUpdatedEventArgs(Dane newValue)
        {
            this.newValue = newValue;
        }

        /// <summary>
        /// Gets the updated value
        /// </summary>
        public Dane NewValue
        {
            get
            {
                return this.newValue;
            }
        }
    }



    public class Turnusy
    {
        string turnus;
        string odKiedy;
        string doKiedy;
        int il_dzieci;
        int il_wych;
        string kierownik;

        public Turnusy(string tur, string odK, string doK, string kier, int ilDz, int ilWy)
        {
            this.turnus = tur;
            this.odKiedy = odK;
            this.doKiedy = doK;
            this.kierownik = kier;
            this.il_dzieci = ilDz;
            this.il_wych = ilWy;
        }
        public Turnusy()
        {
            this.turnus = "";
            this.odKiedy = "";
            this.doKiedy = "";
            this.kierownik = "";
            this.il_dzieci = 0;
            this.il_wych = 0;
        }

        public void AddTurnus(string tur, string odK, string doK, string kier, int ilDz, int ilWy)
        {
            this.turnus = tur;
            this.odKiedy = odK;
            this.doKiedy = doK;
            this.kierownik = kier;
            this.il_dzieci = ilDz;
            this.il_wych = ilWy;
        }
        public string GetTurnus()
        {
            return this.turnus;
        }
        public string GetOd()
        {
            return this.odKiedy;
        }
        public string GetDo()
        {
            return this.doKiedy;
        }
        public string GetKierownik()
        {
            return this.kierownik;
        }
        public int GetIlDzieci()
        {
            return this.il_dzieci;
        }
        public int GetIlWych()
        {
            return this.il_wych;
        }
    }

    static class Program
    {
        public static byte XOR_KEY = 10;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static public string XorString(string input, byte key)
        {
            string tmp = null;
            char[] byteArray = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                byteArray[i] = (char)(input[i] ^ key);
            }
            tmp = new string(byteArray);
            return tmp;
        }
    }
}
