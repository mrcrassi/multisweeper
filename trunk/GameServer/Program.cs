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

                // Register the Shoe class as a well-known (Server-Activated) type 
                RemotingConfiguration.RegisterWellKnownServiceType(
                    typeof(Game), "gamestate.soap",
                    WellKnownObjectMode.Singleton);

                // Keep the server running until <Enter> is pressed
                Console.WriteLine("Game Server is running. Press <Enter> to quit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
