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
    public partial class FromSeedDialogBox : Form
    {
        public FromSeedDialogBox()
        {
            InitializeComponent();
        }

        // Property, gets and sets the value for the From Seed feature.
        public int FromSeedRandom
        {
            get
            {
                return (int)numericUpDownFromSeed.Value;
            }
            set
            {
                numericUpDownFromSeed.Value = value;
            }
        }

        private void buttonRandomize_Click(object sender, EventArgs e)
        {

        }
    }
}
