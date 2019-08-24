using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// True if the High Score control should be visible
        /// </summary>
        public bool HighScoreVisible { get; set; }

        

        //HighScoreInstance
        public static HighScoreViewModel HighScoreInstance { get; set; }

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
        /// Command to Exit Game
        /// </summary>
        public ICommand ExitCommand { get; set; }

        /// <summary>
        /// Hides the new game menu
        /// </summary>
        public ICommand HideNewGameCommand { get; set; }

        /// <summary>
        /// Command to show the High Score menu
        /// </summary>
        public ICommand HighScoreCommand { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuViewModel()
        {
            // Load Scores
            HighScoreInstance = new HighScoreViewModel();

            HighScoreInstance.RestoreHighScoreData();
            

            // Commands for buttons
            StartEasyCommand = new RelayCommand(async () => await StartAsync(Difficulty.Easy));
            StartIntermediateCommand = new RelayCommand(async () => await StartAsync(Difficulty.Medium));
            StartDifficultCommand = new RelayCommand(async () => await StartAsync(Difficulty.Hard));
            ContinueCommand = new RelayCommand(ContinueButton);
            HighScoreCommand = new RelayCommand(HighScoreButton);
            ExitCommand = new RelayCommand(ExitButton);

            // Commands for navigation in MenuPage
            NewGameCommand = new RelayCommand(NewGameButton);
            HideNewGameCommand = new RelayCommand(HideNewGame);

        }

        #endregion

        #region Command Methods

        private void HideNewGame()
        {
            NewGameVisible = false;
            HighScoreVisible = false;
        }

        public void NewGameButton()
        {
            NewGameVisible ^= true;
        }

        public void HighScoreButton()
        {
            HighScoreVisible ^= true;
        }

        /// <summary>
        /// Loads game from stored JSON file
        /// </summary>
        public void ContinueButton()
        {
            BoardViewModel.ContinueGame = true;
            BoardViewModel.NewGame = false;

            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Board);
        }

        /// <summary>
        /// Exit game
        /// </summary>
        public void ExitButton()
        {
            Environment.Exit(0);
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

        #region Helper Methods

        

        #endregion

    }
}
