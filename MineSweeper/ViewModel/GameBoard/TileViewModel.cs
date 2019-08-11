using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MineSweeper
{
    /// <summary>
    /// View model for one induvidual tile
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TileViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// If the tile is flagged
        /// </summary>
        [JsonProperty]
        public bool Flagged { get; set; }

        /// <summary>
        /// Number of mines next to this tile.
        /// -1 indicates it holds a mine??
        /// </summary>
        [JsonProperty]
        public int Number { get; set; }

        /// <summary>
        /// The index of the tile
        /// </summary>
        [JsonProperty]
        public int Index { get; set; }

        /// <summary>
        /// If the tile has been opened or not
        /// </summary>
        [JsonProperty]
        public bool Opened { get; set; }


        #endregion

        #region Commands

        /// <summary>
        /// Command to flag a tile
        /// </summary>
        public ICommand FlagCommand { get; set; }

        /// <summary>
        /// Command to open a tile
        /// </summary>
        public ICommand OpenCommand { get; set; }

        /// <summary>
        /// Command to open surrounding non-flag tiles if number of surrounding flags is equal to number
        /// </summary>
        public ICommand OpenSurroundingCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TileViewModel()
        {
            // Generate commands
            FlagCommand = new RelayCommand(FlagTile);
            OpenCommand = new RelayCommand(OpenTile);
            OpenSurroundingCommand = new RelayCommand(OpenSurrounding);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// When flagging a tile
        /// </summary>
        public void FlagTile()
        {
            
            if (!Opened && !BoardViewModel.GameInstance.FirstTile)
            {                
                Flagged ^=true;
                if (Flagged) BoardViewModel.GameInstance.Mines--;
                else BoardViewModel.GameInstance.Mines++;

            }
        }

        /// <summary>
        /// When opening a tile
        /// </summary>
        public void OpenTile()
        {
            //First tile
            if (BoardViewModel.GameInstance.FirstTile)
            {                
                BoardViewModel.GameInstance.StartingTile = Index;
                BoardViewModel.GameInstance.FirstTile = false;

                // Distribute randomly all mines
                BoardViewModel.GameInstance.PlaceMines();

                // Place Numbers into 
                BoardViewModel.GameInstance.PlaceNumbers();

                BoardViewModel.GameInstance.Time.Start();
               
            }

            //Open tile
            if (!Flagged)
            {

                Opened = true;
                //Open all tiles around opened tile except flagged if 0
                if (Number == 0)
                {
                    OpenSurrounding();                   
                }

                if (Number == -1)
                {
                    //Game lost
                    BoardViewModel.GameInstance.Time.Stop();
                    BoardViewModel.GameInstance.GameRunning = false;
                    BoardViewModel.GameInstance.GameLost = true;
                }
                else
                {
                    BoardViewModel.GameInstance.OpenedTiles++;
                    
                    if(BoardViewModel.GameInstance.OpenedTiles == (BoardViewModel.GameInstance.Rows * BoardViewModel.GameInstance.Columns - BoardViewModel.GameInstance.StartingMines))
                    {
                        //Game won
                        BoardViewModel.GameInstance.Time.Stop();
                        BoardViewModel.GameInstance.GameRunning = false;
                        BoardViewModel.GameInstance.GameWon = true;
                    }
                }

            }

        }

        /// <summary>
        /// When pressing an already opened tile to open surrounding tiles
        /// </summary>
        public void OpenSurrounding()
        {
            int index;
            //Check if number of surrounding flags is equal to number
            int surroundingFlags = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    index = Index + j + BoardViewModel.GameInstance.Columns * i;

                    if (BoardViewModel.GameInstance.InsideBoard(i, j, Index))
                    {
                        if (BoardViewModel.GameInstance.Tiles[index].Flagged) surroundingFlags++;
                    }
                    
                }

            }
            //Open surrounding tiles if true
            if (surroundingFlags == Number)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        index = Index + j + BoardViewModel.GameInstance.Columns * i;
                        //Open tile
                        if (BoardViewModel.GameInstance.InsideBoard(i, j,Index))
                        {
                            if (!BoardViewModel.GameInstance.Tiles[index].Flagged && !BoardViewModel.GameInstance.Tiles[index].Opened)
                            {
                                BoardViewModel.GameInstance.Tiles[index].Opened = true;
                                if (BoardViewModel.GameInstance.Tiles[index].Number == -1)
                                {
                                    //Game lost
                                    BoardViewModel.GameInstance.Time.Stop();
                                    BoardViewModel.GameInstance.GameRunning = false;
                                    BoardViewModel.GameInstance.GameLost = true;
                                }

                                if (BoardViewModel.GameInstance.Tiles[index].Number == 0) BoardViewModel.GameInstance.Tiles[index].OpenSurrounding();

                                BoardViewModel.GameInstance.OpenedTiles++;

                                if (BoardViewModel.GameInstance.OpenedTiles == (BoardViewModel.GameInstance.Rows * BoardViewModel.GameInstance.Columns - BoardViewModel.GameInstance.StartingMines))
                                {
                                    //Game won
                                    BoardViewModel.GameInstance.Time.Stop();
                                    BoardViewModel.GameInstance.GameRunning = false;
                                    BoardViewModel.GameInstance.GameWon = true;
                                }
                                
                            }                                               
                        }           
                    }
                }
            }
        }

        #endregion
    }
}
