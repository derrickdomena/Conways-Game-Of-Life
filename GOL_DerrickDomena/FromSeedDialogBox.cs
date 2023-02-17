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
        // NumericUpDownFromSeed acceptable max num is set to 100,000,000.
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

        // When the Randomize Button is clicked it will save a random number between 0 and numericUpDownFromSeed maximum to its Value.
        private void buttonRandomize_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            numericUpDownFromSeed.Value = random.Next(0, (int)numericUpDownFromSeed.Maximum);
        }
    }
}
