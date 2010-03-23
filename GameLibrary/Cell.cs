/*
 *  Author: Nicholas Lozon
 *  Date:   March 22, 2010
 *  Description: Defines all properties of a cell.
 *  Changes:
 *      March 23, 2010 - Nick Lozon - Added a flag for if the cell is a mine.
 *              Added a mutator for unrevealed mines counter. This class will be moved
 *          to the GameServer so the client can't access it.
 *  Todo:
 *      Move this class to the GameServer and create an ICell interface in the Library.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public class Cell
    {
        // Members
        private int m_unrevealedMines;  // For non-mine cells, defines the number of
                                        //  unrevealed mines whose border exists on it.
        private bool m_isMine; // Defines the cell as a mine

        // Accessors
        public int UnrevealedMines
        {
            get
            {
                return m_unrevealedMines;
            }
            set
            {
                m_unrevealedMines = value;
            }
        }

        public bool IsMine
        {
            get
            {
                return m_isMine;
            }
        }
    }
}
