using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameLibrary;

using System.Runtime.Remoting; // RemotingConfiguration

namespace GameClient
{
    public partial class FormGame : Form
    {
        private IGame m_gameState;

        public FormGame()
        {
            InitializeComponent();

            try
            {
                // Load the remoting configuration file
                RemotingConfiguration.Configure("remoting.config", false);

                m_gameState = (IGame)Activator.GetObject(typeof(IGame),
                    "http://localhost:10000/gamestate.soap");

                MessageBox.Show(m_gameState.PlayerOneScore.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
