/*
 *  Author: Nicholas Lozon
 *  Date:   March 29, 2010
 *  Description: Interface which holds implementation for the server.
 *  Changes:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public interface ICallback
    {
        void UpdateBoardCallback();
    }
}
