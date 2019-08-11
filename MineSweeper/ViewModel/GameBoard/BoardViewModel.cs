using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

namespace MineSweeper
{
    /// <summary>
    /// 
    /// </summary>
    public class BoardViewModel : BaseViewModel
    {
        #region Private Properties
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        private static void OnStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        private static int _mines;

        private static bool _gameRunning;

        private static bool _gameLost;

        private static bool _gameWon;
        #endregion

        #region Public Properties

        /// <summary>
        /// The array of tiles used for binding
        /// </summary>
        public static List<TileViewModel> Tiles { get; set; }

        /// <summary>
        /// The 2d array of tiles
        /// </summary>
        public static int[,] TilesHolder { get; set; }


        /// <summary>
        /// How many rows of tiles
        /// </summary>
        public static int Rows { get; set; }

        /// <summary>
        /// How many columns of tiles
        /// </summary>
        public static int Columns { get; set; }

        /// <summary>
        /// Time after game start
        /// </summary>
        public static DispatcherTimer Time { get; set; }

        /// <summary>
        /// Visual display time
        /// </summary>
        public int DisplayTime { get; set; }

        /// <summary>
        /// Number of mines remaining
        /// </summary>
        public static int Mines {
            get { return _mines; }
            set
            {
                _mines = value;
                OnStaticPropertyChanged("Mines");
            }
        }

        /// <summary>
        /// Number of starting mines
        /// </summary>
        public static int StartingMines { get; set; }

        /// <summary>
        /// True if the game is running, false when game lost or finished
        /// </summary>
        public static bool GameRunning {
            get { return _gameRunning; }
            set
            {
                _gameRunning = value;
                OnStaticPropertyChanged("GameRunning");
            }
        }

        /// <summary>
        /// True if the game is lost
        /// </summary>
        public static bool GameLost
        {
            get { return _gameLost; }
            set
            {
                _gameLost = value;
                OnStaticPropertyChanged("GameLost");
            }
        }

        /// <summary>
        /// True if the game is Won
        /// </summary>
        public static bool GameWon
        {
            get { return _gameWon; }
            set
            {
                _gameWon = value;
                OnStaticPropertyChanged("GameWon");
            }
        }

        /// <summary>
        /// True if the first tile is not pressed
        /// </summary>
        public static bool FirstTile { get; set; } = true;

        /// <summary>
        /// The first selected tile at the start of the game
        /// </summary>
        public static int StartingTile { get; set; }

        /// <summary>
        /// The difficulty of the game
        /// </summary>
        public static Difficulty GameDifficulty { get; set; }

        /// <summary>
        /// Number of opened non-mine tiles
        /// </summary>
        public static int OpenedTiles { get; set; }

        /// <summary>
        /// True is it is a new game
        /// </summary>
        public static bool NewGame { get; set; }

        #endregion

        #region Commands

        public ICommand RestartCommand { get; set; }

        #endregion

        #region Constructor

        public BoardViewModel()
        {
            if (NewGame)
            {
                // Init 
                switch (GameDifficulty)
                {
                    case Difficulty.Easy:
                        Rows = 8;
                        Columns = 10;
                        Mines = 12;
                        break;
                    case Difficulty.Medium:
                        Rows = 12;
                        Columns = 16;
                        Mines = 36;
                        break;
                    case Difficulty.Hard:
                        Rows = 16;
                        Columns = 30;
                        Mines = 99;
                        break;
                    default:
                        break;
                }
                StartingMines = Mines;
                OpenedTiles = 0;

                // Set up timer
                DisplayTime = 0;
                Time = new DispatcherTimer();

                Tiles = new List<TileViewModel>();
                TilesHolder = new int[Rows + 2, Columns + 2];

                InitiateBlankBoard();

                FirstTile = true;
                GameLost = false;
                GameWon = false;
                GameRunning = true;               
            }
            //Continue Game
            else
            {              
                
            }
            // Generate commands
            RestartCommand = new RelayCommand(Restart);

            // Setup timer
            
            Time.Tick += Time_Tick;
            Time.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion

        #region Command Methods

        public void Restart()
        {
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Menu);
        }

        #endregion

        #region Helper Functions
        private void Time_Tick(object sender, EventArgs e)
        {
            DisplayTime++;
        }

        /// <summary>
        /// Places all mines on game board, excluding numbers around opening tile
        /// </summary>
        public static void PlaceMines()
        {
            
            int mines = Mines;

            //Exclude numbers around opening tile
            var exclude = new HashSet<int>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (InsideBoard(i, j, StartingTile)) exclude.Add(StartingTile + j + Columns * i);
                }
            }
            var range = Enumerable.Range(0, Rows * Columns).Where(i => !exclude.Contains(i));
            var rand = new Random();


            while (mines > 0)
            {
                
                int index = rand.Next(0, Rows * Columns - exclude.Count);
                int rndNumber = range.ElementAt(index);

                int rndColumn = (rndNumber % Columns) + 1;
                int rndRow = (rndNumber / Columns) + 1;


                if (TilesHolder[rndRow, rndColumn] != -1)
                {
                    TilesHolder[rndRow, rndColumn] = -1;
                    mines--;
                }
            }
        }
        
        /// <summary>
        /// Searches for mines, and increment all surrounding non-mine tiles by 1 
        /// Improved run time compared to counting surrounding mines of all tiles 
        /// </summary>
        public static void PlaceNumbers()
        {
            // Calculate numbers into TilesHolder
            for (int i = 1; i < Rows + 1; i++)
            {
                for (int j = 1; j < Columns + 1; j++)
                {
                    // Find the mines, then increment the numbers around it
                    if (TilesHolder[i, j] == -1)
                        AddNumbers(i, j);                   
                }
            }
            
            // Place numbers from TilesHolder to Tiles
            for (int i = 1; i < Rows + 1; i++)
            {
                for (int j = 1; j < Columns + 1; j++)
                {
                    int index = (j - 1) + (i - 1) * Columns;
                    Tiles[index].Number = TilesHolder[i, j];
                }                   
            }
        }

        /// <summary>
        /// Increment all non-mine tiles by 1
        /// </summary>
        /// <param name="row">Tile row</param>
        /// <param name="col">Tile Column</param>
        public static void AddNumbers(int row, int col)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (TilesHolder[row + i, col + j] != -1)
                        TilesHolder[row + i, col + j]++;
                }
            }
        }

        /// <summary>
        /// Initiate a blank board
        /// </summary>
        public void InitiateBlankBoard()
        {
            for(int i = 0; i < Rows * Columns; i++)
            {
                Tiles.Add(new TileViewModel { Number = 0, Index = i, Opened = false });
            }
        }

        /// <summary>
        /// Checks if the given offset tile is inside the board
        /// </summary>
        /// <param name="i">Vertical offset from Index</param>
        /// <param name="j">Horizontal offset from Index</param>
        /// <returns></returns>
        public static bool InsideBoard(int i, int j, int tileIndex)
        {
            bool insideBoard = true;
            int index = tileIndex + j + Columns * i;
            //index outside right edge?
            if ((tileIndex + 1) % Columns == 0 && j == 1) insideBoard = false;
            //index outside left edge?
            else if ((tileIndex + 1) % Columns == 1 && j == -1) insideBoard = false;
            //index outside top or bottom edge?
            else if (index < 0 || index >= Columns * Rows) insideBoard = false;

            return insideBoard;
        }


        /*
         * 
         * Attempt to store and restore class for use with Continue function
         * Not working currently because of difficulties with static members.
         * 
        public void StoreAllData()
        {
            //Convert a copy of current instance to Json format
            string output = JsonConvert.SerializeObject(DeepCopy());
            
            //Save output to file




        }
        
        public void RestoreAllData()
        {
            //Get data from file
            string output = "Get from JSON file";

            //Deserialize from JSON to .NET
            BoardViewModel deserializedProduct = JsonConvert.DeserializeObject<BoardViewModel>(output);

            //Put data into class

        }

        public BoardViewModel DeepCopy()
        {
            BoardViewModel copy = (BoardViewModel)this.MemberwiseClone();

            //Problem with deep copying static members...

            return copy;
        }
        */

        #endregion
    }
}
