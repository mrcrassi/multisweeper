/*
 * Author:	Nicholas Lozon
 * Date:	March 22, 2010
 * Details:	Registers the http channel and handles the gamestate.
 * Changes:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary;

using System.Runtime.Remoting;  // RemotingConfiguration class

namespace GameServer
{
    class Program
    {
        Game m_gameState;

        static void Main(string[] args)
        {
            try
            {
                // Load the remoting configuration file
                RemotingConfiguration.Configure("GameServer.exe.config", false);

                // Keep the server running until <Enter> is pressed
                Console.WriteLine("Game Server is running. Press <Enter> to quit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                // TODO: Catch "port number in use" error.
                Console.WriteLine(ex.Message);
            }
        }
    }
}
