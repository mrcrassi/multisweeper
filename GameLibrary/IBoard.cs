/*
 *  Author: Nicholas Lozon
 *  Date:   March 23, 2010
 *  Description: Client interface to the board object.
 *  Changes:
 *      March 23, 2010 - Initial creation.
 *      Added member and methods: BoardWidth, BoardHeight, RevealedCells.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public interface IBoard
    {
        int BoardWidth { get; }
        int BoardHeight { get; }
        List<Cell> RevealedCells { get; }
    }
}
