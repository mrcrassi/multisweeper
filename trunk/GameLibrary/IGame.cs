/*
 *  Author: Nicholas Lozon
 *  Date:   March 22, 2010
 *  Description: Client interface to the server game object.
 *  Changes:
 *      March 22, 2010 - Added member and methods: PlayerOneScore, PlayerTwoScore,
 *          PlayerTurn, RevealedCells, revealBomb, revealCell.
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
        List< List<Cell> > RevealedCells { get; }

        // Methods
        /*
         * Author:  Nicholas Lozon
         * Date:    March 22, 2010
         * Description: Player attempts to reveal a bomb on a unrevealed cell.
         * Changes: 
         */
        void revealBomb();

        /*
         * Author:  Nicholas Lozon
         * Date:    March 22, 2010
         * Description: Player attempts to reveal an empty cell.
         * Changes: 
         */
        void revealCell();
    }
}
