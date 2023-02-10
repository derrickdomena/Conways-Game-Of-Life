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

        //Interval
        #region Timer Interval in Milliseconds
        public int GetIntervalNum()
        {
            return (int)numericUpDownTimerInterval.Value;
        }

        public void SetIntervalNum(int interval)
        {
            numericUpDownTimerInterval.Value = interval;
        }

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

        //Width
        #region Width of Universe in Cells
        public int GetUniverseWidth()
        {
            return (int)numericUpDownWidthUniverse.Value;
        }

        public void SetUniverseWidth(int universeWidth)
        {
            numericUpDownWidthUniverse.Value = universeWidth;
        }
        #endregion

        //Height
        #region Heigth of Universe in Cells
        public int GetUniverseHeight()
        {
            return (int)numericUpDownHeightUniverse.Value;
        }

        public void SetUniverseHeight(int universeHeight)
        {
            numericUpDownHeightUniverse.Value = universeHeight;
        }
        #endregion

    }
}
