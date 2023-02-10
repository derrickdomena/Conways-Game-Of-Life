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
        public int GetNumber()
        {
            return (int)numericUpDownNumber.Value;
        }

        public void SetNumber(int number)
        {
            numericUpDownNumber.Value = number;
        }
        #endregion

    }
}
