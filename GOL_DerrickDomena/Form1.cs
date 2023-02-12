using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace GOL_DerrickDomena
{
    public partial class Form1 : Form
    {
        // Variable Definitions and Initializations.
        #region Variable Definitions and Initializations

        // Static values for universe width and height
        private static int universeWidth = 30;
        private static int universeHeight = 30;

        // The universe array and scratchPad array
        bool[,] universe = new bool[universeWidth, universeHeight];
        bool[,] scratchPad = new bool[universeWidth, universeHeight];

        // Drawing colors
        Color gridColor = Color.Gray;
        Color cellColor = Color.LightGray;
        Color gridx10Color = Color.Black;

        // The Timer class
        Timer timer = new Timer();

        // Memberfield variables
        // Keeps track of generation and alive counts.
        int generationCount = 0;
        int aliveCount = 0;

        // Status strip variables
        #region StatusStrip Variables
        // Default is true, to enable viewing always just in case button is never clicked.
        // Keeps track of toggleing the view for the NeighborCount tool strip button.    
        bool viewNeighborCountClicked = true;

        // Keeps track of toggleing the view for the Grid tool strip button.
        bool viewGridClicked = true;

        // Keeps track of toggleing the view for the HUD tool strip button.
        bool viewHUDClicked = true;
        
        #endregion

        // Keeps track of the generation that the user inputs for Run to generation.
        int targetGeneration = 0;
        #endregion

        // InitializeComponent and timer setup.
        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;

            // Settings Menu Properties
            #region Properties for Saving Settings
            // Reading Properties
            // Back Color
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            // Cell Color
            cellColor = Properties.Settings.Default.CellColor;
            // Grid Color
            gridColor = Properties.Settings.Default.GridColor;
            // Timer Interval
            timer.Interval = Properties.Settings.Default.TimerInterval;
            // Universe Width
            universeWidth = Properties.Settings.Default.UniverseWidth;
            // Universe Height
            universeHeight = Properties.Settings.Default.UniverseHeight;
            #endregion
        }

        // Next Generation
        // Calculate the next generation of cells
        #region NextGeneration
        private void NextGeneration()
        {
            // Sets alive count to 0 on NextGeneration call.
            aliveCount = 0;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    //Neighbor count
                    int count = 0;
                    if (toroidalToolStripMenuItem1.Checked && !finiteToolStripMenuItem.Checked)
                    {
                        count = CountNeighborsToroidal(x, y);
                    }
                    else if (!toroidalToolStripMenuItem1.Checked && finiteToolStripMenuItem.Checked)
                    {
                        count = CountNeighborsFinite(x, y);
                    }

                    //Rules
                    // Checks living cells
                    if (universe[x, y] == true)
                    {
                        //a. Living cells with less than 2 living neighbors die in the next generation.
                        //b. Living cells with more than 3 living neighbors die in the next generation.
                        if (count < 2 || count > 3)
                        {
                            scratchPad[x, y] = false;
                        }
                        //c. Living cells with 2 or 3 living neighbors live in the next generation.
                        else if (count == 2 || count == 3)
                        {
                            scratchPad[x, y] = true;
                            // Increment alive count
                            aliveCount++;
                        }
                    }
                    //Checks dead cells
                    //d. Dead cells with exactly 3 living neighbors live in the next generation.
                    else if (universe[x, y] == false)
                    {
                        if (count == 3)
                        {
                            scratchPad[x, y] = true;
                            // Increment alive count
                            aliveCount++;
                        }
                        // Sets position to false if neighbors not equal to 3
                        else if (count != 3)
                        {
                            scratchPad[x, y] = false;
                        }
                    }
                }
            }

            // Copy from scratchPad to universe
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Increment generation count
            generationCount++;

            // Update status strip generationCount
            toolStripStatusLabelGenerations.Text = "Generations = " + generationCount.ToString();

            // Update status strip aliveCount
            toolStripStatusLabelAlive.Text = "Alive = " + aliveCount.ToString();

            //Invalidate the graphics panel
            graphicsPanel1.Invalidate();
        }
        #endregion

        // Count Neighbors
        #region Count Neighbors
        // Count NeighborsFinite
        #region CountNeighborsFinite
        // A finite universe where the cells live only within the predetermined border.
        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // If checks for surrounding cells and adds count when current cell is alive.
                    if (xCheck >= 0 && xCheck < xLen && yCheck >= 0 && yCheck < yLen)
                    {
                        if (universe[xCheck, yCheck] == true)
                        {
                            count++;
                        }
                    }
                }
            }
            // If the middle cell is alive then minus 1 the current cell.
            if (universe[x, y])
            {
                count--;
            }
            return count;
        }
        #endregion

        //Count NeighborsToroidal
        #region CountNeighborsToroidal
        // A infinite universe where the cells wrap around as if the universe is a sphere.
        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    // Calculates the xCheck and yCheck for surrounding cells and adds count when current cell is alive.
                    int xCheck = (x + yOffset + xLen) % xLen;
                    int yCheck = (y + xOffset + yLen) % yLen;

                    if (universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }
                }
            }
            // If the middle cell is alive, subtract 1
            if (universe[x, y])
            {
                count--;
            }
            return count;
        }
        #endregion
        #endregion

        // Events
        #region Events

        // Timer Tick
        #region Timer Tick
        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Event Run 
            if (targetGeneration == 0)
            {
                NextGeneration();
            }
            // Event Run to Generation, if not reached generations.
            else if (targetGeneration > generationCount)
            {
                NextGeneration();
            }
            // Event Run to Generation, target genrations was reached.
            else
            {
                targetGeneration = 0;
                timer.Enabled = false;
            }
        }
        #endregion

        #endregion

        // Graphics Panel
        #region Graphics Panel

        // Graphics Panel Paint
        #region Graphics Panel Paint
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    Rectangle cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Font and stringFormat for dead cells and alive cells.
                    Font font = new Font("Arial", 8f);                  
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Change the method in which we draw live cells.
                    int neighbors = 0;
                    if (toroidalToolStripMenuItem1.Checked && !finiteToolStripMenuItem.Checked)
                    {
                        neighbors = CountNeighborsToroidal(x, y);
                    }
                    else if (!toroidalToolStripMenuItem1.Checked && finiteToolStripMenuItem.Checked)
                    {
                        neighbors = CountNeighborsFinite(x, y);
                    }
                    
                    // Neighbor Count
                    // If statement that checks if viewNeighborCountClicked is true and that neighbors isn't equal to 0.
                    if (viewNeighborCountClicked && neighbors != 0)
                    {
                        Brush viewNeighborbrush = Brushes.Green;
                        
                        // Fill the cell with the number of neighbors and sets their colors
                        if (universe[x, y] == true && (neighbors == 1 || neighbors > 3))
                        {
                            viewNeighborbrush = Brushes.Red;                          
                        }
                        else if (universe[x, y] == false && neighbors != 3)
                        {
                            viewNeighborbrush = Brushes.Red;                         
                        }
                        e.Graphics.DrawString(neighbors.ToString(), font, viewNeighborbrush, cellRect, stringFormat);
                                           
                    }

                    // Grid
                    // If statement that checks if viewGridClicked is true.
                    if (viewGridClicked)
                    {
                        // Outline the cell with a pen
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                        // Outlines the gridx10 
                        // Still needs work, when resizing the screen it doesn't work properly.
                        //if (cellRect.X % 10 == 0 && cellRect.Y % 10 == 0)
                        //{
                        //    Pen gridx10Pen = new Pen(gridx10Color, 2);
                        //    e.Graphics.DrawRectangle(gridx10Pen, cellRect.X, cellRect.Y, cellRect.Width * 100, cellRect.Height * 100);
                        //}
                    }
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            //gridx10Pen.Dispose();
            cellBrush.Dispose();

            // Update status strip TimeInterval
            toolStripStatusLabelTimeInterval.Text = "Interval = " + timer.Interval.ToString();
          
        }
        #endregion


        // Graphics Panel MouseClick
        #region Graphics Panel MouseClick
        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                if (universe[x, y])
                {
                    aliveCount++;
                }
                else
                {
                    aliveCount--;
                }


                // Update status strip generations
                toolStripStatusLabelAlive.Text = "Alive = " + aliveCount.ToString();

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        #endregion

        // File MenuStrip
        #region File MenuStrip  

        //New
        #region New
        // Resets the universe and satus strip objects.
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            // Resets the generation and alive count
            generationCount = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + generationCount.ToString();
            aliveCount = 0;
            toolStripStatusLabelAlive.Text = "Alive = " + aliveCount.ToString();

            // Resets the universe
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            // Calls graphicsPanel1 Invalidate to update panel.
            graphicsPanel1.Invalidate();
        }
        #endregion

        //Exit
        #region Exit
        // Closes the application.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #endregion

        // View MenuStrip
        #region View MenuStrip

        // HUD
        #region Show HUD
        private void hudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewHUDClicked == true)
            {
                viewHUDClicked = false;
            }
            else
            {
                viewHUDClicked = true;
            }
            graphicsPanel1.Invalidate();
        }
        #endregion

        // Neighbor Count
        #region Show NeighborCount
        //Turns on and off the view for the Neighbor Count
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewNeighborCountClicked == true)
            {
                viewNeighborCountClicked = false;
            }
            else
            {
                viewNeighborCountClicked = true;
            }
            graphicsPanel1.Invalidate();
        }
        #endregion

        // Grid
        #region Show Grid
        //Turns on and off the view of the grid
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewGridClicked == true)
            {
                viewGridClicked = false;
            }
            else
            {
                viewGridClicked = true;
            }
            graphicsPanel1.Invalidate();
        }
        #endregion    

        // Finite ToolStrip
        #region Finite ToolStrip
        // Checks if the Finite check box is true.
        // If Finite mode is checked, then Toroidal mode will be set to false and unchecked.
        private void finiteToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (finiteToolStripMenuItem.Checked == true)
            {
                toroidalToolStripMenuItem1.Checked = false;
                finiteToolStripMenuItem.Checked = !toroidalToolStripMenuItem1.Checked;
            }           
        }
        #endregion

        // Toroidal ToolStrip
        #region Toroidal ToolStrip
        // Checks if the Toroidal check box is true.
        // If Toroidal mode is checked, then Finite mode will be set to false and unchecked.
        private void toroidalToolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
        {
            if (toroidalToolStripMenuItem1.Checked == true)
            {
                finiteToolStripMenuItem.Checked = false;
                toroidalToolStripMenuItem1.Checked = !finiteToolStripMenuItem.Checked;
            }
        }
        #endregion

        #endregion

        // Run MenuStrip
        #region Run MenuStrip

        // Start
        #region Start ToolStrip
        // Starts the timer
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Sets the timer to true
            timer.Enabled = true;
        }
        #endregion

        // Pause
        #region Pause ToolStrip
        // Pauses the timer
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Sets the timer to false
            timer.Enabled = false;
        }
        #endregion

        // Next
        #region Next ToolStrip
        // Calls NextGeneration once
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Calls next generation once
            NextGeneration();
        }
        #endregion

        // To
        #region To ToolStrip
        //Run to Generation Dialog Box
        private void toToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToDialogBox toDialogBox = new ToDialogBox();
            // Sets the current generationCount + 1, since we can't go back a generation on GOL.          
            toDialogBox.Number = generationCount + 1;

            // Gets the generation from the numericUpDown and gives it to targetGeneration.
            if (DialogResult.OK == toDialogBox.ShowDialog())
            {               
                targetGeneration = toDialogBox.Number;
                // Then sets the timer to true.
                timer.Enabled = true;
            }
        }
        #endregion

        #endregion

        // Settings MenuStrip
        #region Settings MenuStrip

        // Color - Back Color
        #region BackColor ToolStrip
        private void backColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog backColorTool = new ColorDialog();

            backColorTool.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == backColorTool.ShowDialog())
            {
                graphicsPanel1.BackColor = backColorTool.Color;
            }
        }
        #endregion

        // Color - Cell Color
        #region CellColor ToolStrip
        private void cellColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog cellColorTool = new ColorDialog();

            cellColorTool.Color = cellColor;

            if (DialogResult.OK == cellColorTool.ShowDialog())
            {
                cellColor = cellColorTool.Color;
            }
            graphicsPanel1.Invalidate();
        }
        #endregion

        // Color - Grid Color
        #region GridColor ToolStrip
        private void gridColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog gridColorTool = new ColorDialog();

            gridColorTool.Color = gridColor;

            if (DialogResult.OK == gridColorTool.ShowDialog())
            {
                gridColor = gridColorTool.Color;
            }
            graphicsPanel1.Invalidate();
        }
        #endregion

        // Options
        #region Options ToolStrip  
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialogBox optionsDialogBox = new OptionsDialogBox();

            // Sets the time interval
            optionsDialogBox.IntervalNum = timer.Interval;
            // Sets the universe width
            optionsDialogBox.UniverseWidth = universeWidth;
            // Sets the universe height
            optionsDialogBox.UniverseHeight = universeHeight;
            if (DialogResult.OK == optionsDialogBox.ShowDialog())
            {
                timer.Interval = optionsDialogBox.IntervalNum;
                universeWidth = optionsDialogBox.UniverseWidth;
                universeHeight = optionsDialogBox.UniverseHeight;
            }
            // Change the size of the universe and scratchPad
            universe = new bool[universeWidth, universeHeight];
            scratchPad = new bool[universeWidth, universeHeight];

            // Call graphicsPanel1 Invalidate to re-paint the screen.
            graphicsPanel1.Invalidate();
        }


        #endregion

        #endregion

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Update the Property
            Properties.Settings.Default.BackColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridColor = gridColor;

            Properties.Settings.Default.TimerInterval = timer.Interval;
            Properties.Settings.Default.UniverseWidth = universeWidth;
            Properties.Settings.Default.UniverseHeight = universeHeight;

            // Take the memory representation of the file and write it out
            Properties.Settings.Default.Save();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();

            // Reading the Property
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;

            timer.Interval = Properties.Settings.Default.TimerInterval;
            universeWidth = Properties.Settings.Default.UniverseWidth;
            universeHeight = Properties.Settings.Default.UniverseHeight;
          
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();

            // Reading the Property
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;

            timer.Interval = Properties.Settings.Default.TimerInterval;
            universeWidth = Properties.Settings.Default.UniverseWidth;
            universeHeight = Properties.Settings.Default.UniverseHeight;
           
        }
    }
}
