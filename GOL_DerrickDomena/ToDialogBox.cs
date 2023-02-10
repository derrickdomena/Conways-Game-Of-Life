using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOL_DerrickDomena
{
    public partial class ToDialogBox : Form
    {
        public ToDialogBox()
        {
            InitializeComponent();
        }

        // To Dialog Box
        #region Run to generation
        // Property, gets and sets the value for the Run to generation feature.
        public int Number
        {
            get 
            {
                return (int)numericUpDownNumber.Value;
            }

            set 
            { 
                numericUpDownNumber.Value = value; 
            }
        }
        #endregion

    }
}
