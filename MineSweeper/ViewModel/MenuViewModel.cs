using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MineSweeper
{
    public class MenuViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// True if the New Game control should be visible
        /// </summary>
        public bool NewGameVisible { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to start easy game
        /// </summary>
        public ICommand StartEasyCommand { get; set; }

        /// <summary>
        /// Command to start medium game
        /// </summary>
        public ICommand StartIntermediateCommand { get; set; }

        /// <summary>
        /// Command to start difficult game
        /// </summary>
        public ICommand StartDifficultCommand { get; set; }

        /// <summary>
        /// Command to show the New game menu
        /// </summary>
        public ICommand NewGameCommand { get; set; }

        /// <summary>
        /// Command to Continue Game
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        /// <summary>
        /// Hides the new game menu
        /// </summary>
        public ICommand HideNewGameCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuViewModel()
        {
            // Create commands
            StartEasyCommand = new RelayCommand(async () => await StartAsync(Difficulty.Easy));
            StartIntermediateCommand = new RelayCommand(async () => await StartAsync(Difficulty.Medium));
            StartDifficultCommand = new RelayCommand(async () => await StartAsync(Difficulty.Hard));

            NewGameCommand = new RelayCommand(NewGameButton);

            HideNewGameCommand = new RelayCommand(HideNewGame);

            ContinueCommand = new RelayCommand(ContinueButton);
        }

        #endregion

        #region Command Methods

        private void HideNewGame()
        {
            NewGameVisible = false;
        }

        public void NewGameButton()
        {
            NewGameVisible ^= true;
        }

        public void ContinueButton()
        {
            //BoardViewModel.NewGame = false;

            //IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Board);

        }

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync(Difficulty difficulty)
        {
            // Go to game
            BoardViewModel.GameDifficulty = difficulty;
            BoardViewModel.NewGame = true;

            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Board);

            await Task.Delay(1);

        }

        #endregion
    }
}
