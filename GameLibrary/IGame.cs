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
        List<Cell> RevealedCells { get; }

        // Methods
        void revealBomb();
        void revealCell();
    }
}
