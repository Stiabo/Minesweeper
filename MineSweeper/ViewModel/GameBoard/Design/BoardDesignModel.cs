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
        public static BoardDesignModel Instance => new BoardDesignModel();

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BoardDesignModel()
        {
            Instance.Rows = 10;
            Instance.Columns = 8;

            Instance.Tiles = new List<TileViewModel>();
            Instance.TilesHolder = new int[Rows + 2, Columns + 2];

            Instance.InitiateBlankBoard();
        }


        #endregion

        
    }
}
