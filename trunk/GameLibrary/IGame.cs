/*
 *  Author: Nicholas Lozon
 *  Date:   March 22, 2010
 *  Description: Client interface to the server game object.
 *  Changes:
 *      March 22, 2010 - Added member and methods: PlayerOneScore, PlayerTwoScore,
 *          PlayerTurn, RevealedCells, revealBomb, revealCell.
 *      March 23, 2010 - Removed RevealedCells member - this has been adapted into the Board class.
 *          Updated revealBomb and revealCells parameters to take two ints for X and Y location.
 *          Renamed revealBomb to revealMine.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary
{
    public interface IGame
    {
        // Members
        int PlayerOneScore { get; }
        int PlayerTwoScore { get; }
        String PlayerTurn { get; }
        IBoard Board { get; }

        // Methods
        /*
         * Author:  Nicholas Lozon
         * Date:    March 22, 2010
         * Description: Player attempts to reveal a mine on a unrevealed cell.
         * Changes: 
         */
        void revealMine(int locX, int locY);

        /*
         * Author:  Nicholas Lozon
         * Date:    March 22, 2010
         * Description: Player attempts to reveal an empty cell.
         * Changes: 
         */
        void revealCell(int locX, int locY);
    }
}
