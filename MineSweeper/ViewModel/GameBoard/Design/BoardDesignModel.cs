using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// 
    /// </summary>
    public class BoardDesignModel : BoardViewModel
    {
        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static BoardDesignModel Instance { get; set; }

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BoardDesignModel()
        {
            Instance = new BoardDesignModel
            {
                Rows = 10,
                Columns = 8,

                Tiles = new List<TileViewModel>(),
                TilesHolder = new int[Rows + 2, Columns + 2],
                

            };
            Instance.InitiateBlankBoard();
        }

       
        #endregion


    }
}
