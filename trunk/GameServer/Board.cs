/*
 *  Author: Nicholas Lozon
 *  Date:   March 23, 2010
 *  Description: Board class to encapsulate board and cell functions.
 *  Implements: IBoard
 *  Changes:
 *      March 23, 2010  Initial creation.
 *          Added member and methods: BoardWidth, BoardHeight, Cells, RevealedCells.
 *          Added function surroundingCells which takes a cell parameter and returns
 *      a list of cells surrounding it.
 *          Added revealMine functon - will reveal a mine and return true or false.
 *          Added revealCell functon - needs functionality.
 *          Added getCell function - takes an X and Y location and find the appropriate
 *      cell in the 1 dimensional list
 *          Added function revealMineBorders which adds all cells around a revealed mine
 *      to the revealed list. The cells must not contain any other unrevealed mines borders.
 *          Added function revealCellBorder which clears all empty cells up to and including
 *      borders of unrevealed mines. This recursive.
 *          Created constructor and initiates members.
 *  Todo:
 *      Populate cells list in constructor.
 *      Randomly add mines.
 *      Add functionility to revealCell()
 *      Decrementing cells unrevealed mine counter requires a mutator, which will
 *          need to be hidden from the user as an Interface. The Cell class will
 *          be moved into the GameServer.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameLibrary;

namespace GameServer
{
    public class Board : IBoard
    {
        // Members
        private int m_boardWidth, m_boardHeight;
        private List<Cell> m_revealedCells, m_cells;

        // Accessors
        public int BoardWidth
        {
            get { return m_boardWidth; }
        }

        public int BoardHeight
        {
            get { return m_boardHeight; }
        }

        public List<Cell> RevealedCells
        {
            get { return m_revealedCells; }
        }

        public List<Cell> Cells
        {
            get { return m_cells; }
        }

        // Constructor
        public Board(int width, int height)
        {
            // Set board dimensions
            m_boardWidth = width;
            m_boardHeight = height;

            // Create cell lists
            m_cells = new List<Cell>();
            m_revealedCells = new List<Cell>();

            // TODO: Populate cell list.
            // TODO: Add mines randomly.
        }

        // Public Methods
        
        // Author:  Nicholas Lozon
        // Date:    March 24, 2010
        // Returns: List<Cell> - The surrounding cells of the cell parameter
        // Details: Since the cells list is a 1 dimensional array to represent a 2 dimensional grid,
        //              math calculations (modulus and division) are used against the board dimensions
        //              to determine whether a cell exists or not in all the locations around the given
        //              cell.
        //          Since arrays start at 0, the relative index of the given cell is increased by 1 to
        //              handle the base 10 calculations. When adding a cell to the return list, we use
        //              the regular index of the given cell.
        public List<Cell> surroundingCells(Cell cell)
        {
            List<Cell> cells = new List<Cell>(); // list to return
            int ind = m_cells.IndexOf(cell) + 1; // offset by one to handle math calculations
            int relInd = ind + 1;

            // top left
            if (relInd % m_boardWidth > 1 && relInd > m_boardWidth)
                cells.Add(m_cells[ind - m_boardWidth - 1]);

            // top
            if (relInd > m_boardWidth)
                cells.Add(m_cells[ind - m_boardWidth]);

            // top right
            if (relInd > m_boardWidth && relInd % m_boardWidth != 0)
                cells.Add(m_cells[ind - m_boardWidth + 1]);

            // left
            if (relInd % m_boardWidth > 1)
                cells.Add(m_cells[ind - 1]);

            // right
            if (relInd % m_boardWidth != 0)
                cells.Add(m_cells[ind + 1]);

            // bottom left
            if (relInd % m_boardWidth > 1 && Math.Ceiling((double)relInd / m_boardWidth) < m_boardHeight)
                cells.Add(m_cells[ind + m_boardWidth - 1]);

            // bottom
            if (Math.Ceiling((double)relInd / m_boardWidth) < m_boardHeight)
                cells.Add(m_cells[ind + m_boardWidth]);

            // bottom right
            if (Math.Ceiling((double)relInd / m_boardWidth) < m_boardHeight && relInd % m_boardWidth != 0)
                cells.Add(m_cells[ind + m_boardWidth + 1]);

            return cells;
        }

        // Author:  Nicholas Lozon
        // Date:    March 24, 2010
        // Returns: bool - Whether a mine has been revealed or not.
        // Details: Translate the X and Y coordinates into an index for the appropriate cell in
        //          the 1 dimensional array. Check if it is a mine - if true, reveal the borders
        //          and return true, else return false.
        public bool revealMine(int locX, int locY)
        {
            // Get the cell of the given location
            Cell cell = getCell(locX, locY);

            // Reveal the cell
            m_revealedCells.Add(cell);

            // Check if it is a mine
            if (cell.IsMine)
            {
                revealMineBorders(cell);
            }
            else
                return false; // A mine has NOT been revealed

            return true; // A mine as been revealed
        }

        public bool revealCell(int locX, int locY)
        {
            // TODO: Need functionality
            return true;
        }

        // Helper Methods

        // Reveals all cells around a mine who no long have a reference to an unrevealed mine
        private void revealMineBorders(Cell chosenCell)
        {
            // loop through every surrounding cell when choosing a mine, reveale the cell
            //  if thereis no unreavealed mines related to it and it is not a mine.
            foreach (Cell cell in surroundingCells(chosenCell))
            {
                if (cell.IsMine != true) // not a mine
                {
                    cell.UnrevealedMines -= 1; // decrement the cells unrevealed mines counter

                    if (cell.UnrevealedMines == 0)
                        m_revealedCells.Add(cell); // unreveal a cell with not unrevealed mines associated with it
                }
            }
        }

        // Recursively reveals cell borders
        private void revealCellBorders(Cell chosenCell)
        {
            // loop through every surrounding cell when choosing a mine, reveale the cell
            //  if thereis no unreavealed mines related to it and it is not a mine.
            foreach (Cell cell in surroundingCells(chosenCell))
                if (cell.UnrevealedMines == 1)
                {
                    m_revealedCells.Add(cell);

                    if (cell.UnrevealedMines == 0)
                        revealCellBorders(cell); // recursive call
                }
        }

        // Gets the corresponding cell in the 1 dimensional array approrpiate to the corresponding x and y
        //  values based on the board dimensions.
        private Cell getCell(int locX, int locY)
        {
            // Multiply y less 1 by the width of the board, add x and subtract 1 to get the index
            return m_cells[(locY - 1) * m_boardWidth + locX - 1];
        }
    }
}
