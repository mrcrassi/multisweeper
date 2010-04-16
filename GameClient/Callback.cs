/*
 * Author:	Nicholas Lozon
 * Date:	March 29, 2010
 * Details:	Class for the server to callback on.
 * Changes:
 *		April 12, 2010 - Richard Blair
 *			- Removed reference to the Window, we can't directly use it anyways.
 *          - Created 2 Delegates: UpdatePlayerGrid, UpdatePlayerScore
 *          - Added a reference to the Dispatcher for the thread the object is created in.
 *              The purpose of this class is to be the place to call the callback methods in the form
 *              meaning the window's thread will be the one creating it. Therefore we will have the
 *              dispatcher for the window's thread.
 *          - Changed UpdateBoardCallback to use the delegates.
 *          - Added getters/setters for the instances of the delegates]
 *      April 16, 2010
 *			- Added functionality for the ChatMessage callback.
 */
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Linq;
using System.Text;

using MultiSweeper.GameLibrary;

namespace GameClient
{
    public class Callback : MarshalByRefObject, ICallback
    {

        #region delegates
		public delegate void UpdatePlayerGrid(String msg);
		public delegate void UpdatePlayerScore();
        public delegate void GameMessageDelegate(String msg);
        public delegate void ChatMessageDelegate(String msg);
        #endregion

        //references to the delegates
        private UpdatePlayerGrid m_updateGrid;
        private UpdatePlayerScore m_updateScore;
		private GameMessageDelegate m_gameMessage;
		private ChatMessageDelegate m_chatMessage;
        
        //this will be set to the same dispatcher which the contructor was created with.
        private Dispatcher m_curDispatch;

        public UpdatePlayerGrid UpdateGrid
        {
            get { return m_updateGrid; }
            set { m_updateGrid = value; }
        }

        public UpdatePlayerScore UpdateScore
        {
            get { return m_updateScore; }
            set { m_updateScore = value; }
        }

        public Callback(UpdatePlayerGrid grid, UpdatePlayerScore score, GameMessageDelegate msg, ChatMessageDelegate chat)
        {
            m_curDispatch = Dispatcher.CurrentDispatcher;
            m_updateGrid += grid;
            m_updateScore += score;
            m_gameMessage += msg;
            m_chatMessage += chat;
        }

        /*
         * Author:	Nicholas Lozon
         * Method: UpdateBoardCallback
         * Details:	Calls the callback methods in the window
         * Dependencies: m_updateGrid, m_updateScore
         * Changes: April 12, 2010 (Richard Blair)
         *          - Changed this to use the dispatcher, and the delegates
         */
		public void UpdateBoardCallback(String msg)
        {
            m_curDispatch.BeginInvoke(m_updateGrid, DispatcherPriority.Normal, msg);
			m_curDispatch.BeginInvoke(m_updateScore, DispatcherPriority.Normal);
        }
        
        /*
		 * Author:	Nicholas Lozon
		 * Method:	GameMessage
		 * Details:	Displays a message to the user from the server.
		 */
		public void GameMessage(String msg)
		{
			m_curDispatch.BeginInvoke(m_gameMessage, DispatcherPriority.Normal, msg);
		}
		
		public void ChatMessage(String msg)
		{
			m_curDispatch.BeginInvoke(m_chatMessage, DispatcherPriority.Normal, msg);
		}
    }
}