/*
 *  Author: Nicholas Lozon
 *  Date:   March 22, 2010
 *  Description: Server class to handle all game logic.
 *  Implements: IGame
 *  Changes:
 *      March 22, 2010 - Added member and methods: PlayerOneScore, PlayerTwoScore,
 *          PlayerTurn, RevealedCells, revealBomb, revealCell.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Contexts; // Synchronization
using GameLibrary;

namespace GameServer
{
    [Synchronization]
    public class Game : MarshalByRefObject, IGame // Derive to use as MarshalByRef and implement IGame
    {
        // Private attributes
        private String playerTurn;
        private int m_playerOneScore;
        private int m_playerTwoScore;
        private List<Cell> m_revealedCells;
        private List<Cell> m_unrevealedCells;

        // Accessors
        public String PlayerTurn
        {
            get { return playerTurn; }
        }

        public int PlayerOneScore
        {
            get { return m_playerOneScore; }
        }

        public int PlayerTwoScore
        {
            get { return m_playerTwoScore; }
        }

        public List<Cell> RevealedCells
        {
            get { return m_revealedCells; }
        }

        // Constructor
        public Game()
        {
            
        }

        // Public methods
        public void revealCell()
        {
        }

        public void revealBomb()
        {
        }

        // Helper methods
    }
}
