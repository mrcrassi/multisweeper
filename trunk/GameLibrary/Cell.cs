/*
 *  Author: Nicholas Lozon
 *  Date:   March 22, 2010
 *  Description: Defines all properties of a cell.
 *  Changes:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public class Cell
    {
        private int m_unrevealedMines;

        public int UnrevealedMines
        {
            get
            {
                return m_unrevealedMines;
            }
        }
    }
}
