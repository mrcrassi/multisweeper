/*
 * Author:	Nicholas Lozon
 * Date:	March 23, 2010
 * Details:	Client interface to the board object.
 * Changes:
 *		March 23, 2010
 *			- Initial creation.
 *			- Added member and methods: BoardWidth, BoardHeight, RevealedCells.
 *		April 7, 2010
 *			- List<ICell> converted to ICell[] for serialization.
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
        Cell[] ClientCells { get; }
    }
}
