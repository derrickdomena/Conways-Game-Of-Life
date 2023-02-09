using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace GOL_DerrickDomena
{
    public partial class Form1 : Form
    {
        // The universe array
        bool[,] universe = new bool[20, 20];
        // The scratchPad array
        bool[,] scratchPad = new bool[20, 20];      

        // Drawing colors
        Color gridColor = Color.Gray;
        Color cellColor = Color.LightGray;

        // The Timer class
        Timer timer = new Timer();

        // Keeps track of the generation count
        int generations = 0;

        // Keeps track if the viewNeighborCount tool strip button was clicked.
        // Default is true, to enable viewing always just in case button is never clicked.
        bool viewNeighborCountClicked = true;

        // Keeps track if the viewGrid tool strip button was clicked.
        // Default is true, to enable viewing always just in case button is never clicked.
        bool viewGridClicked = true;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            //timer.Enabled = true; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {      
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    //Neighbor count
                    int count = CountNeighborsFinite(x, y);
                    //int count = CountNeighborsToroidal(x, y);

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
                        }                    
                    }
                    //Checks dead cells
                    //d. Dead cells with exactly 3 living neighbors live in the next generation.
                    else if (universe[x, y] == false)
                    { 
                        if (count == 3)
                        {
                            scratchPad[x, y] = true;
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
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            //Invalidate the graphics panel
            graphicsPanel1.Invalidate();
        }

        // Count Neighbors
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
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xCheck == 0 && yCheck == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            if (universe[x, y] == true)
            {
                count--;
            }
            return count;
        }

        private int CountNeighborsToroidal(int x, int y)
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
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xLen = -1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yLen = -1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xLen = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yLen = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        //Events
        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }
  
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

                    Font font = new Font("Arial", 8f);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
             
                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);             
                    }

                    // if statement that checks if viewNeighborCount is true.
                    if (viewNeighborCountClicked)
                    {
                        int neighbors = CountNeighborsFinite(x, y);

                        // Fill the cell with the number of neighbors and sets their colors
                        if (universe[x, y] == true)
                        {                          
                            if (neighbors == 2 || neighbors == 3)
                            {                           
                                e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Green, cellRect, stringFormat);
                            }
                            else if (neighbors == 1 || neighbors > 3)
                            {
                                
                                e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Red, cellRect, stringFormat);
                            }
                        }
                        else if (universe[x, y] == false && neighbors != 0)
                        {
                            if (neighbors == 3)
                            {
                                e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Green, cellRect, stringFormat);
                            }
                            else if (neighbors != 3)
                            {
                                e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Red, cellRect, stringFormat);
                            }
                        }                 
                    }

                    // if statement that checks if viewGrid is true.
                    if (viewGridClicked)
                    {                   
                        // Outline the cell with a pen
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

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

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Start
        //Starts the timer
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Sets the timer to true
            timer.Enabled = true;
        }

        //Pause
        //Pauses the timer
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Sets the timer to false
            timer.Enabled = false;
        }

        //Next
        //Calls NextGeneration once
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Calls next generation once
            NextGeneration();
        }

        //New
        //Empties the Universe
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            graphicsPanel1.Invalidate();
        }

        //View - Neighbor Count
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

        //View - Grid
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
    }
}
