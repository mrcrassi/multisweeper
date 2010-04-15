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
 *		April 10, 2010
 *			- Added a list of Guids for client identification
 *			- Added m_playerTurn guid member for player turn
 *			- Added m_inProgress to identify if the game is in progress
 *			- Registering the client callback now returns their Guid
 *			- Updated client functions to take a Guid parameter
 *		April 14, 2010
 *			- Regions ftw.
 *			- Added functionality to revealCell() and revealMine() which ensures
 *				game state and player turn
 *			- Added helper methods for checking player turn and game state
 *			- Added client function to forfeit game
 *			- Added alot of messaging for clients on game ending
 *			- Added helper method to change player turn
 *			- Added helper method to call the end of game
 *		April 15, 2010
 *			- Removed unused member String for player turn
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Contexts; // Synchronization
using MultiSweeper.GameLibrary;

namespace MultiSweeper
{
namespace GameServer
{
    [Synchronization]
    public class Game : MarshalByRefObject, IGame // Derive to use as MarshalByRef and implement IGame
    {
        #region Members
        private int m_playerOneScore;
        private int m_playerTwoScore;
        private Board m_board;
        private List<ICallback> m_clientCallbacks = new List<ICallback>();
        private List<Guid> m_guids = new List<Guid>();
        private Guid m_playerTurn;
		private bool m_inProgress = true;
		#endregion
        
        #region Accessors
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

		public bool InProgress
		{
			get { return m_inProgress; }
		}
		#endregion
		
		#region Constructors
		// Constructor
        public Game()
        {
            m_board = new Board(10, 10, 0.15);

            //Todo: Set the player turn
        }
        #endregion
		
		#region Public Methods
		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Callable by the client to register their function for callbacks.
		 * Parameters:
		 *		callback - reference to the ICallback in the game libaray.
		 */
        public Guid RegisterClientCallback(ICallback callback)
        {
			// Create and new Guid
			Guid guid = System.Guid.NewGuid();

			// Player 1 goes first
			if (m_guids.Count == 0)
				m_playerTurn = guid;
				
			try
			{
				// Add the callback
				m_clientCallbacks.Add(callback);
	            
				// Add Guid to list
				m_guids.Add(guid);

				// Update clients
				System.Console.WriteLine("Player "+(m_guids.IndexOf(guid)+1).ToString()+" has connected to the server.");
				Fire_UpdateClients("Player " + (m_guids.IndexOf(guid) + 1).ToString() + " has connected to the server.");
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
            }
            
            return guid;
        }

        /*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Player attempts to reveal a cell (non-mine)
		 * Parameters:
		 *		locX - Column number
		 *		locY - Row number
		 */
        public void revealCell(int locX, int locY, Guid guid)
        {	
			if(safeToPlay() && playersTurn(guid))
			{
				String msg;
				int playerId = m_guids.IndexOf(guid)+1;
				
				if (!m_board.revealCell(locX, locY))
				{
					if(playerId == 1)
						m_playerOneScore -= 5;
					else
						m_playerTwoScore -= 5;
						
					msg = "Player " + playerId.ToString() + " has tripped a mine. (-5)";
				}
				else
					msg = "Player " + playerId.ToString() + " cleared an area."; 

				Fire_UpdateClients(msg);
			}
        }

		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Player attempts to reveal a mine
		 * Parameters:
		 *		locX - Column number
		 *		locY - Row number
		 */
        public void revealMine(int locX, int locY, Guid guid)
        {
			if(safeToPlay() && playersTurn(guid))
			{
				String msg;
				int playerId = m_guids.IndexOf(guid)+1;
				
				if (m_board.revealMine(locX, locY))
				{
					if (playerId == 1)
						m_playerOneScore += 5;
					else
						m_playerTwoScore += 5;
						
					msg = "Player " + playerId.ToString() + " has disarmed a mine. (5)";
				}
				else
				{
					if (playerId == 1)
						m_playerOneScore -= 2;
					else
						m_playerTwoScore -= 2;

					msg = "Player " + playerId.ToString() + " disarmed nothing. (-2)";
				}
				
				Fire_UpdateClients(msg);
			}
        }

		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Player forfeits the game.
		 */
		public void forfeitGame(Guid guid)
		{
			// End game
			m_inProgress = false;
			int playerId = (m_guids.IndexOf(guid) + 1);
			Console.WriteLine("Player " + playerId.ToString() + " has forfeited the game.");
			
			Guid winner;
			if(m_guids[0] != guid)
			{
				winner = m_guids[0];
				m_clientCallbacks.Remove(m_clientCallbacks[1]);
			}
			else
			{
				winner = m_guids[1];
				m_clientCallbacks.Remove(m_clientCallbacks[0]);
			}
			
			Fire_EndGame(winner);
		}
		#endregion

        #region Helper Methods
        // Checks if the player performing the action is the one in turn
		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details:	Checks to see that the player making the request is the same players' who turn it is.
		 */
        private bool playersTurn(Guid guid)
        {
            if(guid != m_playerTurn)
				return false;
			
			return true;
        }
        
        private bool safeToPlay()
        {
			// Make sure we have 2 players
			if(m_guids.Count < 2)
				return false;
				
			return true;
        }
        
        private void changePlayerTurn()
        {
			if(m_guids[0] == m_playerTurn)
				m_playerTurn = m_guids[1];
			else
				m_playerTurn = m_guids[0];
        }

		/*
		 * Author:	Nicholas Lozon
		 * Date:	March 29, 2010
		 * Details: Fired when client relative data is updated and the client is notified through their
		 *			registered callback function.
		 */
        private void Fire_UpdateClients(String msg)
        {		
			if(m_board.RevealedCellsCount == m_board.ServerCells.Count)
			{
				if(m_playerOneScore > m_playerTwoScore)
					Fire_EndGame(m_guids[0]);
				else if(m_playerOneScore < m_playerTwoScore)
					Fire_EndGame(m_guids[1]);
				else
					Fire_EndGame();
			}

			foreach (ICallback callback in m_clientCallbacks)
				callback.UpdateBoardCallback(msg);
			
			changePlayerTurn();
        }
        
        private void Fire_MessageClients(String msg)
        {
			foreach (ICallback callback in m_clientCallbacks)
				callback.GameMessage(msg);
        }
        
        private void Fire_EndGame(Guid winner)
        {
			String msg = "Player " + (m_guids.IndexOf(winner) + 1).ToString() + " has won the game.";
			Console.WriteLine(msg);
			foreach (ICallback callback in m_clientCallbacks)
				callback.GameMessage(msg);
        }
        
        private void Fire_EndGame()
        {
			String msg = "The game ended in a draw!";
			Console.WriteLine(msg);
			foreach (ICallback callback in m_clientCallbacks)
				callback.GameMessage(msg);
        }
        #endregion
    }
}
}