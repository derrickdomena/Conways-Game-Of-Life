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
    public partial class OptionsDialogBox : Form
    {
        public OptionsDialogBox()
        {
            InitializeComponent();
        }

        // Interval
        #region Timer Interval in Milliseconds
        // Property, gets and sets the value for IntervalNum.
        public int IntervalNum
        {
            get
            {
                return (int)numericUpDownTimerInterval.Value;
            }

            set
            {
                numericUpDownTimerInterval.Value = value;
            }
        }
        #endregion

        // Width
        #region Width of Universe in Cells    
        // Property, gets and sets the value for UniverseWidth.
        public int UniverseWidth
        {
            get
            {
                return (int)numericUpDownWidthUniverse.Value;
            }
            set
            {
                numericUpDownWidthUniverse.Value = value;
            }
        }
        #endregion

        //Height
        #region Heigth of Universe in Cells
        // Property, gets and sets the value for UniverseHeight.
        public int UniverseHeight
        {
            get
            {
                return (int)numericUpDownHeightUniverse.Value;
            }
            set
            {
                numericUpDownHeightUniverse.Value = value;
            }
        }
        #endregion

    }
}
