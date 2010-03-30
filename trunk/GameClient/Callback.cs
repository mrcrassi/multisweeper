/*
 *  Author: Nicholas Lozon
 *  Date:   March 29, 2010
 *  Description: Class for the server to callback on.
 *  Changes:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameLibrary;

namespace GameClient
{
    public class Callback : MarshalByRefObject, ICallback
    {
        private FormGame frm;

        public Callback(FormGame f)
        {
            frm = f;
        }

        public void UpdateBoardCallback()
        {
            // TODO: Use a delegate to update - using the form thread will not work.
            frm.UpdateGame();
        }
    }
}
