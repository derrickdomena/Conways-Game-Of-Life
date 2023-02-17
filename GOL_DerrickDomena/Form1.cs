using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace GOL_DerrickDomena
{
    public partial class Form1 : Form
    {
        // Variable Definitions and Initializations
        #region Variable Definitions and Initializations

        // Static values for universe width and height
        private static int universeWidth = Properties.Settings.Default.UniverseWidth;
        private static int universeHeight = Properties.Settings.Default.UniverseHeight;

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

        // Keeps track of the seed
        int seedCount = 2001;

        // viewMode sets the view for HUD if its Finite or Torodial.
        string viewMode = "";

        // Bool value that keeps track of the randomize seedMethod.
        // If true, then randomize would run From Time
        // If false, then randomize would either be running From Seed or From Current Seed.
        bool seedMethod = true;

        #endregion

        // InitializeComponent
        // Includes Timer Setup and Settings Menu Properties
        public Form1()
        {
            InitializeComponent();

            // Timer Setup
            #region Timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            #endregion

            // Settings Menu Properties
            #region Properties for Saving Settings
            // Reading Properties
            // Back Color
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            // Cell Color
            cellColor = Properties.Settings.Default.CellColor;
            // Grid Color
            gridColor = Properties.Settings.Default.GridColor;
            // Gridx10 Color
            gridx10Color = Properties.Settings.Default.Gridx10Color;
            // Timer Interval
            timer.Interval = Properties.Settings.Default.TimerInterval;
            // Universe Width
            universeWidth = Properties.Settings.Default.UniverseWidth;
            // Universe Height
            universeHeight = Properties.Settings.Default.UniverseHeight;
            #endregion
        }

        // Next Generation
        #region Next Generation
        // Calculate the next generation of cells
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
        // Includes Finite and Toroidal
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

                    // Calculates the xCheck and yCheck for surrounding cells and adds count if the cell is alive.
                    if ((xCheck >= 0 && xCheck < xLen) && (yCheck >= 0 && yCheck < yLen))
                    {
                        if (universe[xCheck, yCheck] == true)
                        {
                            count++;
                        }
                    }
                }
            }
            
            // If the middle cell is alive then subtract one to the current cell.
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

                    // Calculates the xCheck and yCheck for surrounding cells and adds count if the cell is alive.
                    int xCheck = (x + xOffset + xLen) % xLen;
                    int yCheck = (y + yOffset + yLen) % yLen;

                    if (universe[xCheck, yCheck] == true)
                    {
                        count++;
                    }
                }
            }

            // If the middle cell is alive then subtract one to the current cell.
            if (universe[x, y])
            {
                count--;
            }
            return count;
        }
        #endregion
        #endregion

        // Events
        // Includes Timer Tick and Properties Close Event
        #region Events

        // Timer Tick Event
        #region Timer Tick Event
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

        // Close Event
        #region Close Event
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Update the Property

            // Back Color
            Properties.Settings.Default.BackColor = graphicsPanel1.BackColor;
            // Cell Color
            Properties.Settings.Default.CellColor = cellColor;
            // Grid Color
            Properties.Settings.Default.GridColor = gridColor;
            // Gridx10 Color
            Properties.Settings.Default.Gridx10Color = gridx10Color;

            // Timer Interval
            Properties.Settings.Default.TimerInterval = timer.Interval;
            // Universe Width
            Properties.Settings.Default.UniverseWidth = universeWidth;
            // Universe Height
            Properties.Settings.Default.UniverseHeight = universeHeight;

            // Take the memory representation of the file and write it out
            Properties.Settings.Default.Save();
        }
        #endregion

        #endregion

        // Graphics Panel
        // Includes Paint and MouseClick
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
            Pen gridx10Pen = new Pen(gridx10Color, 2);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Font and stringFormat for dead cells and alive cells.
            Font font = new Font("Arial", 8f);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

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
                        // Still needs work, when resizing the screen it draws a string even when gridx10 is not mod 10 = 0.
                        if (cellRect.X % 10 == 0 && cellRect.Y % 10 == 0)
                        {
                            e.Graphics.DrawRectangle(gridx10Pen, cellRect.X, cellRect.Y, cellRect.Width*10, cellRect.Height*10);
                        }                      
                    }          
                }
            }
            //HUD
            if (viewHUDClicked)
            {
                // Font and stringFormat for dead cells and alive cells.
                Font fontHUD = new Font("Arial", 12f);
                StringFormat stringFormatHUD = new StringFormat();
                stringFormatHUD.Alignment = StringAlignment.Near;
                stringFormatHUD.LineAlignment = StringAlignment.Far;
                
                if (finiteToolStripMenuItem.Checked)
                {
                    viewMode = "Finite";
                    e.Graphics.DrawString("Generations: " + generationCount.ToString() + "\n" + "Cell Count: " + aliveCount.ToString() + "\n" + "Boundary Type: " + viewMode + "\n" + "Universe Size: {Width=" + universeWidth.ToString() + ", Height=" + universeHeight.ToString() + "}", fontHUD, Brushes.LightCoral, graphicsPanel1.ClientRectangle, stringFormatHUD);
                }
                else if (toroidalToolStripMenuItem1.Checked)
                {
                    viewMode = "Toroidal";
                    e.Graphics.DrawString("Generations: " + generationCount.ToString() + "\n" + "Cell Count: " + aliveCount.ToString() + "\n" + "Boundary Type: "+ viewMode + "\n" + "Universe Size: {Width=" + universeWidth.ToString() + ", Height=" + universeHeight.ToString() + "}", fontHUD, Brushes.LightCoral, graphicsPanel1.ClientRectangle, stringFormatHUD);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            gridx10Pen.Dispose();
            cellBrush.Dispose();

            // Update status strip TimeInterval
            toolStripStatusLabelTimeInterval.Text = "Interval = " + timer.Interval.ToString();

            // Update status strip Seed
            toolStripStatusLabelSeed.Text = "Seed: " + seedCount.ToString();
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
        // Includes New, Open, Import, Save, and Exit
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
            timer.Enabled = false;

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

        // Open
        #region Open
        // This reads in a file type .cells and writes to the universe if the cell is alive or dead.
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.StartsWith("!"))
                    {
                        continue;
                    }

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    if (!row.Equals("!"))
                    {
                        maxHeight++;
                    }

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                universe = new bool[maxWidth, maxHeight];
                scratchPad = new bool[maxWidth, maxHeight];

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                int yPos = 0;

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.StartsWith("!"))
                    {
                        continue;
                    }

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    if (!row.Equals("!"))
                    {
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {

                            // If row[xPos] is a 'O' (capital O) then
                            // set the corresponding cell in the universe to alive.
                            if (row[xPos].Equals('O'))
                            {
                                universe[xPos, yPos] = true;
                            }
                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                            else if (row[xPos].Equals('.'))
                            {
                                universe[xPos, yPos] = false;
                            }
                        }
                    }

                    yPos++;
                }

                // Close the file.
                reader.Close();

                //Invalidate the graphics panel
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        // Import
        #region Import
        // Works the same way as Open.
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();           
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.StartsWith("!"))
                    {
                        continue;
                    }

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    if (!row.Equals("!"))
                    {
                        maxHeight++;
                    }

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                universe = new bool[maxWidth, maxHeight];
                scratchPad = new bool[maxWidth, maxHeight];

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                int yPos = 0;

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.StartsWith("!"))
                    {
                        continue;
                    }

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    if (!row.Equals("!"))
                    {
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {

                            // If row[xPos] is a 'O' (capital O) then
                            // set the corresponding cell in the universe to alive.
                            if (row[xPos].Equals('O'))
                            {
                                universe[xPos, yPos] = true;
                            }
                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                            else if (row[xPos].Equals('.'))
                            {
                                universe[xPos, yPos] = false;
                            }
                        }
                    }

                    yPos++;
                }

                // Close the file.
                reader.Close();

                //Invalidate the graphics panel
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        // Save
        #region Save 
        // This method writes in a file type .cells and saves the live cells as a "O" (capital letter O), and the dead cells as "." (period).
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!This is my comment.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (universe[x, y] == true)
                        {
                            currentRow += 'O';
                        }
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else if (universe[x, y] == false)
                        {
                            currentRow += '.';
                        }
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
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
        // Includes HUD, Neighbor Count, Grid, Finite, and Toroidal
        #region View MenuStrip

        // HUD ToolStrip
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

        // Neighbor Count ToolStrip
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

        // Grid ToolStrip
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
                viewMode = "Finite";
                toroidalToolStripMenuItem1.Checked = false;
                finiteToolStripMenuItem.Checked = !toroidalToolStripMenuItem1.Checked;            
            }
            graphicsPanel1.Invalidate();
        }

        // If Finite Mode is Clicked and it is also Checked
        // Assume that the user wants to disable Finite Mode and enable Toroidal Mode
        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (finiteToolStripMenuItem.Checked == false)
            {
                toroidalToolStripMenuItem1.Checked = true;
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
                viewMode = "Toroidal";
                finiteToolStripMenuItem.Checked = false;
                toroidalToolStripMenuItem1.Checked = !finiteToolStripMenuItem.Checked;
            }
            graphicsPanel1.Invalidate();
        }

        // If Toroidal Mode is Clicked and it is also Checked
        // Assume that the user wants to disable Toroidal Mode and Enable Finite Mode
        private void toroidalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (toroidalToolStripMenuItem1.Checked == false)
            {
                finiteToolStripMenuItem.Checked = true;
            }
        }
        #endregion

        #endregion

        // Run MenuStrip
        // Includes Start, Pause, Next, and To 
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
        // Includes Color - Back, Cell, Grid, Gridx10. Options, Reset, and Reload
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

        // Color - Gridx10 Color
        #region Gridx10Color ToolString
        private void gridX10ColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog gridx10ColorTool = new ColorDialog();

            gridx10ColorTool.Color = gridx10Color;

            if (DialogResult.OK == gridx10ColorTool.ShowDialog())
            {
                gridx10Color = gridx10ColorTool.Color;
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

                // Change the size of the universe and scratchPad
                universe = new bool[universeWidth, universeHeight];
                scratchPad = new bool[universeWidth, universeHeight];
            }
           
            // Call graphicsPanel1 Invalidate to re-paint the screen.
            graphicsPanel1.Invalidate();
        }


        #endregion

        // Reset
        #region Reset ToolStrip
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();

            // Reading the Property

            // Back Color
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            // Cell Color
            cellColor = Properties.Settings.Default.CellColor;
            // Grid Color
            gridColor = Properties.Settings.Default.GridColor;
            // Gridx10 Color
            gridx10Color = Properties.Settings.Default.Gridx10Color;

            // Timer Interval
            timer.Interval = Properties.Settings.Default.TimerInterval;
            // Universe Width
            universeWidth = Properties.Settings.Default.UniverseWidth;
            // Universe Height
            universeHeight = Properties.Settings.Default.UniverseHeight;

            // Change the size of the universe and scratchPad
            universe = new bool[universeWidth, universeHeight];
            scratchPad = new bool[universeWidth, universeHeight];

            // Call graphicsPanel1 Invalidate to re-paint the screen.
            graphicsPanel1.Invalidate();
        }
        #endregion

        // Reload
        #region Reload ToolStrip
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();

            // Reading the Property
            // Back Color
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            // Cell Color
            cellColor = Properties.Settings.Default.CellColor;
            // Grid Color
            gridColor = Properties.Settings.Default.GridColor;
            // Gridx10 Color
            gridx10Color = Properties.Settings.Default.Gridx10Color;

            // Timer Interval
            timer.Interval = Properties.Settings.Default.TimerInterval;
            // Universe Width
            universeWidth = Properties.Settings.Default.UniverseWidth;
            // Universe Height
            universeHeight = Properties.Settings.Default.UniverseHeight;

            // Change the size of the universe and scratchPad
            universe = new bool[universeWidth, universeHeight];
            scratchPad = new bool[universeWidth, universeHeight];

            // Call graphicsPanel1 Invalidate to re-paint the screen.
            graphicsPanel1.Invalidate();
        }
        #endregion

        #endregion

        // Randomize MenuStrip
        // Includes From Seed, From Current Seed, and From Time
        #region Randomize MenuStrip

        // RandomizeSeed
        // Initializes a random class variable and sets a random amount of cells either alive or dead in the universe.
        private void RandomizeSeed()
        {
            // Initialize a random member.
            Random randSeed = new Random(); // Time, Default
            
            // If seedMethod is set to false, then either From Seed or From Current Seed method was clicked.
            // Then we need to pass a seedCount parameter to randSeed.
            if (seedMethod == false)
            {
                randSeed = new Random(seedCount); // Seed
            }
            else
            {
                // Updates the seedCount when seedMethod is true.
                // If seedCount isn't updated then the status stip bar would not reflect the current seed.
                // Max is based on the max from the numericUpDownFromSeed properties maximum.
                seedCount = randSeed.Next(0, 100000000);
            }
            
            // Keep track of alive count.
            aliveCount = 0;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int next = randSeed.Next(0, 2);                 

                    // Turns on or off the cell in the universe.
                    if (next == 0)
                    {
                        universe[x, y] = false;
                    }
                    else if (next == 1)
                    {
                        universe[x, y] = true;
                        aliveCount++;
                    }
                }
            }

            // Update status strip Seed and alive.
            toolStripStatusLabelSeed.Text = "Seed: " + seedCount.ToString();
            toolStripStatusLabelAlive.Text = "Alive = " + aliveCount.ToString();

            // Re-paint the universe.
            graphicsPanel1.Invalidate();
        }
     
        // From Seed
        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            seedMethod = false;
            FromSeedDialogBox fromSeedDialogBox = new FromSeedDialogBox();

            // Gets the value from seedCount and saves it to FromSeedRandom.
            fromSeedDialogBox.FromSeedRandom = seedCount;

            // Checks if OK was clicked, sets the value for seed and RandomizeSeed is called.
            if (DialogResult.OK == fromSeedDialogBox.ShowDialog())
            {
                seedCount = fromSeedDialogBox.FromSeedRandom;
                RandomizeSeed();
            } 
        }

        // From Current Seed
        private void fromCurrentSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            seedMethod = false;

            // RandomizeSeed is only called here since we are returning a random universe based on the current seed.
            RandomizeSeed();
        }

        // From Time
        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // seedMethod is updated 
            seedMethod = true;

            // RandomizeSeedTime is called here since we are returning a random universe based on time.
            RandomizeSeed();
        }
        #endregion

    }
}
