/*
 * Author:	Nicholas Lozon
 * Date:	March 22, 2010
 * Details:	Server class to handle all game logic.
 * Changes:
 *		March 22, 2010
 *			- Added member and methods: PlayerOneScore, PlayerTwoScore, PlayerTurn,
 *			RevealedCells, revealBomb, revealCell.
 *		March 22, 2010
 *			- Added revealMineBorders and surroundingCells functions.
 *			- Modified Cell arrays to be List< List<Cell> > instead of List< Cell >
 *		March 23, 2010
 *			- Removed cell arrays and revealMineBorders/surroundingCells functions and
 *			put them in the new Board class.
 *			- Added a board members.
 *			- Added constructor
 *			- Adapted revealCell and revealMine functions to take two integer location paremters.
 *			- Added a playersTurn() function to check if the player requesting an action is the
 *			correct player to take a turn.
 *			- Renamed revealBomb to revealMine.
 *		March 29, 2010
 *			- Added function for registering a client callback.
 *			- Added function for firing a callback.
 *			- Added a list of client callback objects.
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
        private List<ICallback> m_clientCallbacks = new List<ICallback>();

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
            get { return (IBoard)m_board; }
        }

        // Constructor
        public Game()
        {
            m_board = new Board(5, 5, 0);

            //Todo: Set the player turn
        }

        // Public methods
		
		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Callable by the client to register their function for callbacks.
		 * Parameters:
		 *		callback - reference to the ICallback in the game libaray.
		 */
        public void RegisterClientCallback(ICallback callback)
        {
            m_clientCallbacks.Add(callback);
        }

        /*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Player attempts to reveal a cell (non-mine)
		 * Parameters:
		 *		locX - Column number
		 *		locY - Row number
		 */
        public void revealCell(int locX, int locY)
        {
            if (m_board.revealCell(locX, locY))
            {
                // TODO: Increment player score
            }
            else
            {
                // TODO: Decrement player score
            }

            Fire_UpdateClients();
        }

		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Player attempts to reveal a mine
		 * Parameters:
		 *		locX - Column number
		 *		locY - Row number
		 */
        public void revealMine(int locX, int locY)
        {
            if (m_board.revealMine(locX, locY))
            {
                // TODO: Increment player score
            }
            else
            {
                // TODO: Decrement player score
            }

            Fire_UpdateClients();
        }

        // Helper methods

        // Checks if the player performing the action is the one in turn
		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Checks to see that the player making the request is the same players' who turn it is.
		 */
        private bool playersTurn()
        {
            // TODO: Need functionality
            return true;
        }

		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details: Fired when client relative data is updated and the client is notified through their
		 *			registered callback function.
		 */
        private void Fire_UpdateClients()
        {
            foreach (ICallback callback in m_clientCallbacks)
                callback.UpdateBoardCallback();
        }
    }
}
