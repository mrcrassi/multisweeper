/*
 * Author:	Nicholas Lozon
 * Date:	March 29, 2010
 * Details:	Interface which holds implementation for the server.
 * Changes:
 *		April 10, 2010
 *			- Added function definition for GameMessage callback
 *			- Updated UpdateBoardCallback to accept a string message
 *		April 16, 2010
 *			- Added function definition for ChatMessage
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiSweeper
{
namespace GameLibrary
{
    public interface ICallback
    {
        void UpdateBoardCallback(String msg);
        void GameMessage(String msg);
        void ChatMessage(String msg);
    }
}
}