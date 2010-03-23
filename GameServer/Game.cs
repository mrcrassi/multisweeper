/*
 *  Author: Nicholas Lozon
 *  Date:   March 22, 2010
 *  Description: Server class to handle all game logic.
 *  Implements: IGame
 *  Changes:
 *      March 22, 2010 - Added member and methods: PlayerOneScore, PlayerTwoScore,
 *          PlayerTurn, RevealedCells, revealBomb, revealCell.
 *      March 22, 2010 - Added revealMineBorders and surroundingCells functions.
 *                     - Modified Cell arrays to be List< List<Cell> > instead of List< Cell >
 *      March 23, 2010 - Removed cell arrays and revealMineBorders/surroundingCells functions and
 *          put them in the new Board class.
 *              Added a board members.
 *              Added constructor
 *              Adapted revealCell and revealMine functions to take two integer location paremters.
 *              Added a playersTurn() function to check if the player requesting an action is the
 *                  correct player to take a turn.
 *              Renamed revealBomb to revealMine.
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
        private Board m_board;

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

        public IBoard Board // return as IBoard for the GameLibrary
        {
            get { return m_board; }
        }

        // Constructor
        public Game()
        {
            m_board = new Board(10, 10);

            //Todo: Set the player turn
        }

        // Public methods

        // Player attempts to reveal a cell (non-mine)
        public void revealCell(int locX, int locY)
        {
            if (m_board.revealCell(locX, locY))
            {
                // TODO: Need functionality
            }
            else
            {
                // TODO: Need functionality
            }
        }

        // Player attempts to reveal a mine
        public void revealMine(int locX, int locY)
        {
            if (m_board.revealMine(locX, locY))
            {
                // TODO: Need functionality
            }
            else
            {
                // TODO: Need functionality
            }
        }

        // Helper methods

        // Checks if the player performing the action is the one in turn
        private bool playersTurn()
        {
            // TODO: Need functionality
            return true;
        }
    }
}
