/*
 * Author:	Richard Blair
 * Date:	April 12, 2010
 * Details:	Client side implementation. Displays information from server.
 * Changes: April 12, 2010:
 *              - Created GUI
 *              - Added functionality to specify server
 *              - Dynamic grid based off of server information
 *              - Wrote functions to get UpdateGame() out of here, and move dealing with delegates into Callback.cs
 *          April 13, 2010:
 *              - Added commenting for file, and function headers.
 *          April 15, 2010
 *				- Removed unused member variable for indexing cell lists
 *			April 16, 2010
 *				- Added functionality for the ChatMessage callback
 *				- Added bold player labels identifying players turn
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

using System.Runtime.Remoting; // RemotingConfiguration

using MultiSweeper.GameLibrary;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //game state of which we are interacting with.
        private IGame m_gameState;
        //our GUID
        private Guid m_guid;
        //buttons used for game play
        private List<Button> m_cellBtns;
		private static Color[] m_colors = { Colors.Blue, Colors.Green, Colors.DarkOrange, Colors.Red, Colors.Red, Colors.Red, Colors.Red };

        public Window1()
        {
            InitializeComponent();

            txtBlkMsgs.Text = "";
            RemotingConfiguration.Configure("remoting.config", false);
            m_cellBtns = new List<Button>();
        }

        /*
		 * Author:	Richard Blair
		 * Date:	April 12, 2010
         * Method: initializeGrid
		 * Details:	Creates the grid dynamicaly based off of the server
		 * Dependencies: IGame m_gameState
		 */
        private void initializeGrid()
        {
            //only if we have a connection do we allow this to happen
            if (m_gameState != null)
            {
                try
                {
                    #region init_grid
                    grdButtonGrid.Children.Clear();
                    m_cellBtns.Clear();

                    //initialize the row/column definitions
                    for (int i = 0; i != m_gameState.Board.BoardWidth; ++i)
                    {
                        grdButtonGrid.ColumnDefinitions.Add(new ColumnDefinition()
                        {
                            Width = new GridLength(1, GridUnitType.Star)
                        });
                    }
                    
					for (int i = 0; i != m_gameState.Board.BoardHeight; ++i)
					{
						grdButtonGrid.RowDefinitions.Add(new RowDefinition()
						{
							Height = new GridLength(1, GridUnitType.Star)
						});
					}

                    //add the buttons to our dynamic grid.
                    for (int y = 0; y != m_gameState.Board.BoardHeight; ++y)
                    {
                        for (int x = 0; x != m_gameState.Board.BoardWidth; ++x)
                        {
                            //create new button
                            Button myButton = new Button();
                            //absolute x,y location for cell.
                            myButton.Tag = new Cell(x + 1, y + 1);
                            //simple name convention "C{x}{y}"
                            myButton.Name = "C" + x.ToString() + y.ToString();
                            //event handlers for left and right click
                            myButton.MouseRightButtonUp += right_Click;
                            myButton.Click += left_Click;
                            myButton.HorizontalAlignment = HorizontalAlignment.Stretch;

                            //set the position within the grid
                            Grid.SetColumn(myButton, x);
                            Grid.SetRow(myButton, y);

                            //add the button to the grid of buttons
                            grdButtonGrid.Children.Add(myButton);
                            
                            //add to local list
                            m_cellBtns.Add(myButton);
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    //although it is bad to show the user the exact error message, I am going to anyways.
                    txtBlkMsgs.Text += "An error has occured: " + ex.Message + Environment.NewLine;
                }
            }
        }

        /*
         * Author:	Richard Blair
         * Date:	April 12, 2010
         * Method: initializeConnection
         * Details:	Attempts to connect to the given ipaddress.
         * Dependencies: IGame m_gameState
         */
        private void initializeConnection()
        {
            try
            {

                #region connect
                // TODO: Remove this for the remoting.config
				m_gameState = (IGame)Activator.GetObject(typeof(IGame),
					"http://"+txtIp.Text+"/gamestate.soap");

				// Register callback
				//m_guid = m_gameState.RegisterClientCallback(new Callback(this));
                
                m_guid = m_gameState.RegisterClientCallback(new Callback(updateGrid, updateScore, gameMessage, chatMessage));
                #endregion
                txtIp.IsEnabled = false;
                btnConnect.IsEnabled = false;
			}
			catch (Exception ex)
			{
                txtBlkMsgs.Text += "An error has occured: " + ex.Message + Environment.NewLine;
			}
        }

        private void txtIp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                initializeConnection();
                initializeGrid();
            }
        }

        /*
         * Author:	Richard Blair
         * Date:	April 12, 2010
         * Method: updateGrid
         * Details:	Updates the grid based off of the cells from m_gameState
         * Dependencies: IGame m_gameState
         */
		private void updateGrid(String msg)
		{
			txtBlkMsgs.Text += msg + Environment.NewLine;
			Cell[] cells = m_gameState.Board.ClientCells;

			int i = 0;
			foreach (Cell c in cells)
			{
				if(c != null)
				{
					//replace the tag
					m_cellBtns[i].Tag = c;
					//change the state of the button
					m_cellBtns[i].IsEnabled = false;

					//update button text
					if (c.IsMine)
						m_cellBtns[i].Content = new TextBlock()
						{
							FontSize = 16,
							Text = "M",
							Foreground = new SolidColorBrush(Colors.Black),
							FontWeight = FontWeights.Bold
						};
					else if (c.PerimitiveMines > 0)
						m_cellBtns[i].Content = new TextBlock()
						{
							FontSize = 16,
							Text = c.PerimitiveMines.ToString(),
							Foreground = new SolidColorBrush(m_colors[c.PerimitiveMines-1]),
							FontWeight = FontWeights.Bold
						};
				}
				++i;
			}
		}

        /*
         * Author:	Richard Blair
         * Date:	April 12, 2010
         * Method: updateScore
         * Details:	Updates the plater scores based off of the cells from m_gameState
         * Dependencies: IGame m_gameState
         */
		private void updateScore()
        {
            txtBlkP1Score.Text = m_gameState.PlayerOneScore.ToString();
            txtBlkP2Score.Text = m_gameState.PlayerTwoScore.ToString();
            if(!m_gameState.PlayerTurn) // player 1 turn
            {
				lblP1.FontWeight = FontWeights.Bold;
				lblP2.FontWeight = FontWeights.Normal;
			}
			else // player 2 turn
			{
				lblP1.FontWeight = FontWeights.Normal;
				lblP2.FontWeight = FontWeights.Bold;
			}
        }
        
        private void gameMessage(String msg)
        {
			MessageBox.Show(msg);
        }

        #region reveal_mine_cell
        /*
         * Author:	Richard Blair
         * Date:	April 12, 2010
         * Method: updateScore
         * Details:	Updates the plater scores based off of the cells from m_gameState
         * Dependencies: IGame m_gameState
         */
        private void right_Click(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Button) 
                && ((Button)sender).Tag.GetType() == typeof(Cell)
                && m_gameState != null)
            {
                //we now know we have the correct types...
                //right click will reveal a bomb
                Cell curCell = (Cell)((Button)sender).Tag;
                m_gameState.revealMine(curCell.LocX, curCell.LocY, m_guid);
            }
        }

        /*
         * Author:	Richard Blair
         * Date:	April 12, 2010
         * Method: updateScore
         * Details:	Updates the plater scores based off of the cells from m_gameState
         * Dependencies: IGame m_gameState
         */
        private void left_Click(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Button) 
                && ((Button)sender).Tag.GetType() == typeof(Cell)
                && m_gameState != null)
            {
                //we now know we have the correct types...
                //left click will reveal a cell
                Cell curCell = (Cell)((Button)sender).Tag;
                m_gameState.revealCell(curCell.LocX, curCell.LocY, m_guid);
            }
        }
        #endregion

        /*
         * Author:	Richard Blair
         * Date:	April 12, 2010
         * Method: btnConnect_Click
         * Details:	Initializes connection and Grid
         * Dependencies: IGame m_gameState
         */
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            initializeConnection();
            initializeGrid();
        }

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				// If the game is in progress, forfeit the game
				if (m_gameState != null && m_gameState.InProgress)
				{
					MessageBoxResult result = MessageBox.Show("Are you sure you want to forfeit?", "Forfeit", MessageBoxButton.OKCancel);

					if (result == MessageBoxResult.OK)
						m_gameState.forfeitGame(m_guid);
					else
						e.Cancel = true;
				}
			}
			catch (System.Net.WebException)
			{
				MessageBox.Show("Lost connection to server." + Environment.NewLine + "The client will now close");
				btnConnect.IsEnabled = true;
			}
		}
		
		private void chatMessage(String msg)
		{
			txtBlkMsgs.Text += msg + Environment.NewLine;
		}
    }
}
