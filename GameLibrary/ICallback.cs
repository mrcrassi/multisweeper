/*
 * Author:	Nicholas Lozon
 * Date:	March 29, 2010
 * Details:	Interface which holds implementation for the server.
 * Changes:
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
        void UpdateBoardCallback();
    }
}
}