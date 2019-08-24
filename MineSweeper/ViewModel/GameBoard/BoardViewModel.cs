using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    [JsonObject(MemberSerialization.OptIn)]
    public class BoardViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// The array of tiles used for binding
        /// </summary>
        [JsonProperty]
        public List<TileViewModel> Tiles { get; set; }

        /// <summary>
        /// The 2d array of tiles
        /// </summary>
        public int[,] TilesHolder { get; set; }


        /// <summary>
        /// How many rows of tiles
        /// </summary>
        [JsonProperty]
        public int Rows { get; set; }

        /// <summary>
        /// How many columns of tiles
        /// </summary>
        [JsonProperty]
        public int Columns { get; set; }

        /// <summary>
        /// Time after game start
        /// </summary>
        public DispatcherTimer Time { get; set; }

        /// <summary>
        /// Visual display time
        /// </summary>
        [JsonProperty]
        public int DisplayTime { get; set; } = 0;

        /// <summary>
        /// Number of mines remaining
        /// </summary>
        [JsonProperty]
        public int Mines { get; set; }

        /// <summary>
        /// Number of starting mines
        /// </summary>
        [JsonProperty]
        public int StartingMines { get; set; }

        /// <summary>
        /// True if the game is running, false when game lost or finished
        /// </summary>
        [JsonProperty]
        public bool GameRunning { get; set; } = true;

        /// <summary>
        /// True if the game is lost
        /// </summary>
        [JsonProperty]
        public bool GameLost { get; set; } = false;

        /// <summary>
        /// True if the game is Won
        /// </summary>
        [JsonProperty]
        public bool GameWon { get; set; } = false;

        /// <summary>
        /// True if the first tile is not pressed
        /// </summary>
        [JsonProperty]
        public bool FirstTile { get; set; } = true;

        /// <summary>
        /// The first selected tile at the start of the game
        /// </summary>
        [JsonProperty]
        public int StartingTile { get; set; }

        /// <summary>
        /// The difficulty of the game
        /// </summary>
        [JsonProperty]
        public static Difficulty GameDifficulty { get; set; }

        /// <summary>
        /// Number of opened non-mine tiles
        /// </summary>
        [JsonProperty]
        public int OpenedTiles { get; set; } = 0;

        /// <summary>
        /// True if it is a new game
        /// </summary>
        public static bool NewGame { get; set; }

        /// <summary>
        /// True if you want to continue game
        /// </summary>
        public static bool ContinueGame { get; set; }

        /// <summary>
        /// A single static Instance of the game
        /// </summary>
        public static BoardViewModel GameInstance { get; set; }


        public static string InstancePath { get; } = "..\\..\\JSON\\GameInstance.json";

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

                // Setup timer
                Time = new DispatcherTimer();
                Time.Tick += Time_Tick;
                Time.Interval = new TimeSpan(0, 0, 1);

                GameInstance = (BoardViewModel)this.MemberwiseClone();

                GameInstance.Tiles = new List<TileViewModel>();
                GameInstance.TilesHolder = new int[Rows + 2, Columns + 2];

                GameInstance.InitiateBlankBoard();

            }
            //Continue Game
            else if (ContinueGame)
            {
                
                RestoreAllData();

                GameInstance.Time = new DispatcherTimer();
                GameInstance.Time.Tick += Time_Tick;
                GameInstance.Time.Interval = new TimeSpan(0, 0, 1);
                if (GameInstance.GameRunning) GameInstance.Time.Start();
            }
            // Generate commands
            RestartCommand = new RelayCommand(Restart);

        }

        #endregion

        #region Command Methods

        public void Restart()
        {
            GameInstance.Time.Stop();
            StoreAllData();            
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Menu);
        }

        #endregion

        #region Helper Functions
        private void Time_Tick(object sender, EventArgs e)
        {
            GameInstance.DisplayTime++;
        }

        /// <summary>
        /// Places all mines on game board, excluding numbers around opening tile
        /// </summary>
        public void PlaceMines()
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
        public void PlaceNumbers()
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
        public void AddNumbers(int row, int col)
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
        public bool InsideBoard(int i, int j, int tileIndex)
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



        /// <summary>
        /// Stores the all data on GameInstance to a JSON file
        /// </summary>
        public void StoreAllData()
        {
            //Convert a copy of current instance to Json format
            string output = JsonConvert.SerializeObject(GameInstance);

            //Save output to file
            //string path = "..\\..\\JSON\\GameInstance.json";
            if(File.Exists(InstancePath)) File.WriteAllText(InstancePath, output);           

        }
        
        /// <summary>
        /// Restores GameInstance from a JSON file
        /// </summary>
        public void RestoreAllData()
        {
            //Get data from file
            //string path = "..\\..\\JSON\\GameInstance.json";
            if (File.Exists(InstancePath))
            {
                string output = File.ReadAllText(InstancePath);

                //Deserialize from JSON to .NET
                ContinueGame = false;
                GameInstance = JsonConvert.DeserializeObject<BoardViewModel>(output);

            }

        }


        #endregion

        #region Destructor
        ~BoardViewModel()
        {
            StoreAllData();
        }
        #endregion
        
    }
}
