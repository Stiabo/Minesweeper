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
    public class TileViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// If the tile is flagged
        /// </summary>
        public bool Flagged { get; set; }

        /// <summary>
        /// Number of mines next to this tile.
        /// -1 indicates it holds a mine??
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// The index of the tile
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// If the tile has been opened or not
        /// </summary>
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
            if (!Opened && !BoardViewModel.FirstTile)
            {                
                Flagged ^=true;
                if (Flagged) BoardViewModel.Mines--;
                else BoardViewModel.Mines++;
            }
        }

        /// <summary>
        /// When opening a tile
        /// </summary>
        public void OpenTile()
        {
            //First tile
            if (BoardViewModel.FirstTile)
            {                
                BoardViewModel.StartingTile = Index;
                BoardViewModel.FirstTile = false;

                // Distribute randomly all mines
                BoardViewModel.PlaceMines();

                // Place Numbers into 
                BoardViewModel.PlaceNumbers();

                BoardViewModel.Time.Start();
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
                    BoardViewModel.Time.Stop();
                    BoardViewModel.GameRunning = false;
                    BoardViewModel.GameLost = true;
                }
                else
                {
                    BoardViewModel.OpenedTiles++;
                    
                    if(BoardViewModel.OpenedTiles == (BoardViewModel.Rows * BoardViewModel.Columns - BoardViewModel.StartingMines))
                    {
                        //Game won
                        BoardViewModel.Time.Stop();
                        BoardViewModel.GameRunning = false;
                        BoardViewModel.GameWon = true;
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
                    index = Index + j + BoardViewModel.Columns * i;

                    if (BoardViewModel.InsideBoard(i, j, Index))
                    {
                        if (BoardViewModel.Tiles[index].Flagged) surroundingFlags++;
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
                        index = Index + j + BoardViewModel.Columns * i;
                        //Open tile
                        if (BoardViewModel.InsideBoard(i, j,Index))
                        {
                            if (!BoardViewModel.Tiles[index].Flagged && !BoardViewModel.Tiles[index].Opened)
                            {
                                BoardViewModel.Tiles[index].Opened = true;
                                if (BoardViewModel.Tiles[index].Number == -1)
                                {
                                    //Game lost
                                    BoardViewModel.Time.Stop();
                                    BoardViewModel.GameRunning = false;
                                    BoardViewModel.GameLost = true;
                                }

                                if (BoardViewModel.Tiles[index].Number == 0) BoardViewModel.Tiles[index].OpenSurrounding();

                                BoardViewModel.OpenedTiles++;

                                if (BoardViewModel.OpenedTiles == (BoardViewModel.Rows * BoardViewModel.Columns - BoardViewModel.StartingMines))
                                {
                                    //Game won
                                    BoardViewModel.Time.Stop();
                                    BoardViewModel.GameRunning = false;
                                    BoardViewModel.GameWon = true;
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
