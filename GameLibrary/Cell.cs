/*
 * Author:	Nicholas Lozon
 * Date:	March 22, 2010
 * Details:	Defines all properties of a cell.
 * Changes:
 *		March 23, 2010
 *			- Added a flag for if the cell is a mine.
 *			- Added a mutator for unrevealed mines counter. This class will be moved
 *			to the GameServer so the client can't access it.
 *		April 7, 2010
 *			- Changed m_unrevealedMines to m_unrevealedPerimitiveMines for clarity
 *			- Added m_perimitiveMines for the total perimitive mines.
 *			- Added m_revealed flag for identifying whether the cell is revealed or not.
 *			- Added m_locX and m_locY for location.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public class Cell : MarshalByRefObject
    {
        // Members
        private int m_perimitiveMines;  // For non-mine cells, thenumber of mines whose borders exists on it.
        private int m_unrevealedPerimitiveMines; // For non-mine cells, defines the number of
                                                 // unrevealed mines whose border exists on it. This is used
                                                 // for revealing cells around mines when it hits 0.
        private bool m_isMine; // Defines the cell as a mine
        private bool m_revealed; // If this cell is revealed
        private int m_locX, m_locY;

        // Constructor
        public Cell(int locX, int locY)
        {
            m_locX = locX;
            m_locY = locY;  
        }

        // Accessors
        public int UnrevealedPerimitiveMines
        {
            get
            {
                return m_unrevealedPerimitiveMines;
            }
            set
            {
                m_unrevealedPerimitiveMines = value;
            }
        }

        public int PerimitiveMines
        {
            get
            {
                return m_perimitiveMines;
            }
            set
            {
                m_perimitiveMines = value;
            }
        }

        public bool IsMine
        {
            get
            {
                return m_isMine;
            }
            set
            {
                m_isMine = value;
            }
        }

        public bool Revealed
        {
            get
            {
                return m_revealed;
            }
            set
            {
                m_revealed = value;
            }
        }

        public int LocX
        {
            get
            {
                return m_locX;
            }
            set
            {
                m_locX = value;
            }
        }

        public int LocY
        {
            get
            {
                return m_locY;
            }
            set
            {
                m_locY = value;
            }
        }
    }
}
