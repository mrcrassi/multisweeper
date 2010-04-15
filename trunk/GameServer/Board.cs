/* Author:	Nicholas Lozon
 * Date:	March 23, 2010
 * Details:	Board class to encapsulate board and cell functions.
 * Changes:
 *		March 23, 2010  Initial creation.
 *			- Added member and methods: BoardWidth, BoardHeight, Cells, RevealedCells.
 *			- Added function surroundingCells which takes a cell parameter and returns
 *			a list of cells surrounding it.
 *			- Added revealMine functon - will reveal a mine and return true or false.
 *			- Added revealCell functon - needs functionality.
 *			- Added getCell function - takes an X and Y location and find the appropriate
 *			cell in the 1 dimensional list
 *			- Added function revealMineBorders which adds all cells around a revealed mine
 *			to the revealed list. The cells must not contain any other unrevealed mines borders.
 *			- Added function revealCellBorder which clears all empty cells up to and including
 *			borders of unrevealed mines. This recursive.
 *			- Created constructor and initiates members.
 *		April 7, 2010
 *			- Changed m_revealedCells and m_cells to m_clientCells and m_serverCells for clarity.
 *			- Implements MarshalByRefObject for serialization.
 *			- Added a mine percentage member for adding mines to a percentage of the board size.
 *			- Added a third parameter (double) to the constructor for mine percentage.
 *			- Updated m_clientCells to return Cell[]
 *			- Added functionality to build the cell list for the table sizes.
 *			- Added functionality to add random mines for the mine percentage.
 *		April 14, 2010
 *			- Regions ftw.
 *			- The Client cells list now returns null values for unrevealed cells, this lowers
 *				client side load for iterating through and comparing cells.
 *			- Added a member to count the number of revealed cells. This is necessary since
 *				the client cells list is always the same size.
 *			- Added a revealCell helper method to encapsulate functionality
 *			- Fixed a logic error where revealCell only returns false when it is a mine
 *			- Fixed reveal cell borders to all cells on borders, not just ones with permitive
 *				mines equal to 1.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MultiSweeper.GameLibrary;

namespace MultiSweeper
{
namespace GameServer
{
    public class Board : MarshalByRefObject, IBoard
    {
        #region Members
        private int m_boardWidth, m_boardHeight;
        private List<Cell> m_serverCells, m_clientCells;
        private double m_minePercentage;
		private int m_revealedCellsCount;

        #endregion

        #region Accessors
		public int RevealedCellsCount
		{
			get { return m_revealedCellsCount; }
			set { m_revealedCellsCount = value; }
		}
		
        public int BoardWidth
        {
            get { return m_boardWidth; }
        }

        public int BoardHeight
        {
            get { return m_boardHeight; }
        }

        public List<Cell> ServerCells
        {
            get { return m_serverCells; }
        }

        public Cell[] ClientCells
        {
            get { return m_clientCells.ToArray(); }
        }
        #endregion

		#region Constructors
		/* Author:     Nicholas Lozon
		 * Date:       March 24, 2010
		 * Details:    Instantiates members, fill m_serverCells and add mines to them.
		 * Parameters:
		 *		width - Board width
		 *		height - Board height
		 *		minePercentage - Percentage of mines on board
		 */
		public Board(int width, int height, double minePercentage)
		{
			// Set board dimensions
			m_boardWidth = width;
			m_boardHeight = height;
			m_minePercentage = minePercentage;

			// Create cell lists
			m_serverCells = new List<Cell>();
			Cell[] test = new Cell[m_boardHeight * m_boardWidth];
			m_clientCells = test.ToList<Cell>();

			// Fill server cells list with coordinates
			for (int y = 0; y != height; ++y) // column
			{
				for (int x = 0; x != width; ++x) // row
					m_serverCells.Add(new Cell(x + 1, y + 1));
			}

			int numCells = width * height;
			// Randomly add mines
			Random rnd = new Random();
			int ranNumber;
			for (int i = 0; i != Math.Round(numCells * minePercentage); ++i)
			{
				// Choose random cells until one is not a mine
				do
				{
					ranNumber = rnd.Next(0, numCells - 1); // 0 to one less the number of cells
				} while (m_serverCells[ranNumber].IsMine);

				// Make the cell a mine
				m_serverCells[ranNumber].IsMine = true;

				// Increment mine counters
				List<Cell> temp = surroundingCells(m_serverCells[ranNumber]);
				foreach (Cell cell in surroundingCells(m_serverCells[ranNumber]))
				{
					++cell.PerimitiveMines;
					++cell.UnrevealedPerimitiveMines;
				}
			}
		}
		#endregion

		#region Public Methods
		/* Author:	Nicholas Lozon
		 * Date:	March 24, 2010
		 * Details:	Since the cells list is a 1 dimensional array to represent a 2 dimensional grid,
		 *			math calculations (modulus and division) are used against the board dimensions
		 *			to determine whether a cell exists or not in all the locations around the given
		 *			cell. Since arrays start at 0, the relative index of the given cell is increased by 1 to
		 *			handle the base 10 calculations. When adding a cell to the return list, we use
		 *			the regular index of the given cell.
		 * Parameters:
		 *		cell - The cell to find the surrounding cells of
		 * Returns:
		 *		List<Cell> - List of surrounding cells.
		 * Changes:
		 *		April 7, 2010
		 *			- The evaluation to find if a cell has a column left of it is (relInd % m_boardWidth != 1)
		 *			and not (relInd % m_boardWidth < 1)
		 *			- ind variable was being incremented by 1, causing index out of range errors
		 */
		public List<Cell> surroundingCells(Cell cell)
		{
			List<Cell> cells = new List<Cell>(); // list to return
			int ind = m_serverCells.IndexOf(cell);
			int relInd = ind + 1; // offset by one to handle math calculations

			// top left
			if (relInd % m_boardWidth != 1 && relInd > m_boardWidth)
				cells.Add(m_serverCells[ind - m_boardWidth - 1]);

			// top
			if (relInd > m_boardWidth)
				cells.Add(m_serverCells[ind - m_boardWidth]);

			// top right
			if (relInd > m_boardWidth && relInd % m_boardWidth != 0)
				cells.Add(m_serverCells[ind - m_boardWidth + 1]);

			// left
			if (relInd % m_boardWidth != 1)
				cells.Add(m_serverCells[ind - 1]);

			// right
			if (relInd % m_boardWidth != 0)
				cells.Add(m_serverCells[ind + 1]);

			// bottom left
			if (relInd % m_boardWidth != 1 && Math.Ceiling((double)relInd / m_boardWidth) < m_boardHeight)
				cells.Add(m_serverCells[ind + m_boardWidth - 1]);

			// bottom
			if (Math.Ceiling((double)relInd / m_boardWidth) < m_boardHeight)
				cells.Add(m_serverCells[ind + m_boardWidth]);

			// bottom right
			if (Math.Ceiling((double)relInd / m_boardWidth) < m_boardHeight && relInd % m_boardWidth != 0)
				cells.Add(m_serverCells[ind + m_boardWidth + 1]);

			return cells;
		}

		/* Authors:	Nicholas Lozon
		 * Date:	March 24, 2010
		 * Details:	Translate the X and Y coordinates into an index for the appropriate cell in
		 *			the 1 dimensional array. Check if it is a mine - if true, reveal the borders
		 *			and return true, else reveal JUST the mine cell and return false.
		 * Parameters:
		 *		locX - Column number
		 *		locY - Row number
		 * Returns:	Boolean - Whether a mine has been revealed or not.
		 * Changes:
		 *		April 7, 2010
		 *			- When a cell is revealed, it is also added to m_clientCells
		*/
        public bool revealMine(int locX, int locY)
        {
            // Get the cell of the given location
            Cell cell = getCell(locX, locY);

            if (cell.Revealed == false)
            {
                // Reveal the cell
                revealCell(cell);

                // Check if it is a mine
                if (cell.IsMine)
                {
                    revealMineBorders(cell);
                }
                else
                    return false; // A mine has NOT been revealed
            }
            else
                return false; // That location is already revealed!

            return true; // A mine as been revealed
        }

		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 24, 2010
		 * Details:	Attempt to reveal a cell as a non-mine, and then reveals the borders.
		 * Parameters:
		 *		locX - Column number
		 *		locY - Row number
		 * Returns: Boolean - If it is a mine, return false - the user failed at revealing a
		 *			non-mine.If it isn't a mine, reveal the cell borders and return true -
		 *			the user correctly revealed a mine.
		 * Changes:
		 *		April 7, 2010
		 *			- Added functionality
		 */
        public bool revealCell(int locX, int locY)
        {
            Cell cell = getCell(locX, locY);

            if (cell.Revealed == false)
            {
                // Reveal the cell
				revealCell(cell);

                // Return false if its a mine
                if(cell.IsMine)
					return false;
				
				// Reveal border if it has no perimitive mines
                if (cell.PerimitiveMines == 0)
                    revealCellBorders(cell);
            }
            else
                return false; // That location is already revealed!

            return true;
        }
		#endregion

		#region Helper Methods
		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 24, 2010
		 * Details:	Reveals all cells around a mine who no long have a reference to an unrevealed mine.
		 * Parameters:
		 *		chosenCell - Cell to reveal the borders of.
		 * Changes:
		 *		April 7, 2010
		 *			- Add a revealed cell to m_clientCells
		 */
        private void revealMineBorders(Cell chosenCell)
        {
            // loop through every surrounding cell when choosing a mine, reveale the cell
            //  if thereis no unreavealed mines related to it and it is not a mine.
            foreach (Cell cell in surroundingCells(chosenCell))
            {
                if (cell.Revealed == false && cell.IsMine != true) // not a mine
                {
                    cell.UnrevealedPerimitiveMines -= 1; // decrement the cells unrevealed mines counter

                    if (cell.UnrevealedPerimitiveMines == 0)
                    {
						revealCell(cell);
                    }
                }
            }
        }

		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 24, 2010
		 * Details:	Recursively reveals cell borders
		 * Parameters:
		 *		chosenCell - Cell to reveal the borders of.
		 * Changes:
		 *		April 7, 2010
		 *			- Add a revealed cell to m_clientCells
		 *			- Recursive call should be made when the revealed cell has NO perimitive mines,
		 *			not unrevealed primitive mines.
		 */
        private void revealCellBorders(Cell chosenCell)
        {
            // loop through every surrounding cell when choosing a mine, reveal the cell
            //  if there is no unreavealed mines related to it and it is not a mine.
            foreach (Cell cell in surroundingCells(chosenCell))
                if (cell.Revealed == false)
                {
					revealCell(cell);

                    if (cell.PerimitiveMines == 0) // Only recurse if the revealed cell has no perimitive mines
                        revealCellBorders(cell); // recursive call
                }
        }

		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 24, 2010
		 * Details:	Gets the corresponding cell in the 1 dimensional array approrpiate to the
		 *			corresponding x and y values based on the board dimensions.
		 */
		private Cell getCell(int locX, int locY)
        {
            // Multiply y less 1 by the width of the board, add x and subtract 1 to get the index
            return m_serverCells[(locY - 1) * m_boardWidth + locX - 1];
        }
        
        private void revealCell(Cell cell) {
			cell.Revealed = true;
			m_clientCells[m_serverCells.IndexOf(cell)] = cell;
			++m_revealedCellsCount;
        }
        #endregion
    }
}
}