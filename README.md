# Conways-Game-Of-Life

Implementation of the below features can be found in the Form1.cs

BASIC FEATURES:

1. Render Conway’s Game of Life in a .NET application. A grid should be rendered representing the individual cells. Cells can be turned on and off by clicking on them with the mouse. Once a group of cells are turned on and the game runs they should live or die according the 4 rules of the game.
 - Living cells with less than 2 living neighbors die in the next generation.
 - Living cells with more than 3 living neighbors die in the next generation.
 - Living cells with 2 or 3 living neighbors live in the next generation.
 - Dead cells with exactly 3 living neighbors live in the next generation.
2. Start, Pause and Next menu items and tool strip buttons. The game should start running by clicking on a Start menu item or a tool strip button. The game should be pause by clicking on a Pause menu item or a tool strip button. If currently paused, the game can be advanced 1 generation by clicking on a Next menu item or a tool strip button.
3. Randomizing the universe. The current universe can be randomly populated from time or from a seed variable. The user should be able to edit the seed variable through a dialog box. Randomizing should occur when a menu item is clicked on.
4. Emptying the universe. The universe should be emptied of all living cells through a New or Clear menu item.
5. Saving the current universe to a text file. The current state and size of the universe should be able to be saved in PlainText file format. The file name should be chosen by the user with a save file dialog box.
6. Opening a previously saved universe. A previously saved PlainText file should be able to be read in and assigned to the current universe. Opening should also resize the current universe to match the size of the file being read.
7. Show the current generation. The current generation should be able to be displayed in a status strip.
8. Show the current number of living cells. The current number of living cells should be displayed in a status strip.
9. Controlling how many milliseconds between new generations. The number of milliseconds between new generations should be adjustable through a dialog box.
10. Controlling the current size of the universe. The width and height of the current universe should be able to be chosen through a modal dialog box.
11. Displaying the neighbor count in each cell. Render the neighbor count for each individual cell. The user should be able to toggle this feature on and off using the View menu.
12. View Menu Items. Implement a View Menu that toggles the grid on an off, toggles the neighbor count display and toggles the heads up display (if the heads up is implemented as an advanced feature.)


ADVANCED FEATURES:

13. Universe boundary behavior. The user should choose how the game is going to treat the edges of the universe. The two basic options would be toroidal (the edges wrap around to the other side) or finite (cells outside the universe are considered dead.)
14. Context sensitive menu. Implement a ContextMenuStrip that allows the user to change various options in the application.
15. Heads up display. A heads up display that indicates current generation, cell count, boundary type, universe size and any other information you wish to display. The user should be able to toggle this display on and off through a View menu and a context sensitive menu (if one is implemented as an advanced feature.)
16. Settings. When universe size, timer interval and color options are changed by the user they should persist even after the program has been closed and then opened again. Also, the user should have two menu items Reset and Reload. Reload will revert back to the last saved settings and Reset will return the applications default settings for these values.
