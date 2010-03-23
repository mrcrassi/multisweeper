/*
 *  Author: Nicholas Lozon
 *  Date:   March 22, 2010
 *  Description: Server class to handle all game logic.
 *  Implements: IGame
 *  Changes:
 *      March 22, 2010 - Added member and methods: PlayerOneScore, PlayerTwoScore,
 *          PlayerTurn, RevealedCells, revealBomb, revealCell.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Contexts; // Synchronization
using GameLibrary;

namespace GameServer
{
    [Synchronization]
    public class Game : MarshalByRefObject, IGame // Derive to use as MarshalByRef and implement IGame
    {
        // Private attributes
        private String playerTurn;
        private int m_playerOneScore;
        private int m_playerTwoScore;
        private List< List<Cell> > m_revealedCells;
        private List< List<Cell> > m_unrevealedCells;
        private int m_boardWidth, m_boardHeight;

        // Accessors
        public String PlayerTurn
        {
            get { return playerTurn; }
        }

        public int PlayerOneScore
        {
            get { return m_playerOneScore; }
        }

        public int PlayerTwoScore
        {
            get { return m_playerTwoScore; }
        }

        public List< List<Cell> > RevealedCells
        {
            get { return m_revealedCells; }
        }

        // Constructor
        public Game()
        {
            
        }

        // Public methods
        public void revealCell()
        {
        }

        public void revealBomb()
        {
        }

        // Helper methods

        // Returns the surrounding cells of the cell parameter
        private List< List<Cell> > surroundingCells(Cell cell)
        {
            List< List<Cell> > cells = new List< List<Cell> >();

            for (int i = -1; i != 2; ++i)
            {
                for (int j = -1; j != 2; ++j)
                {
                    if (i != 0 && j != 0)
                    {
                        cells[i].Add(m_unrevealedCells[i][j]);
                    }
                }
            }

            return cells;
        }

        // Reveals all cells around a mine who no long have a reference to an unrevealed mine
        private void revealMineBorders(Cell mine)
        {
            foreach (List<Cell> row in surroundingCells(mine))
                foreach (Cell borderCell in row)
                    if (borderCell.UnrevealedMines == 0)
                        m_revealedCells[row.IndexOf(borderCell)].Add(borderCell);
        }
    }
}
