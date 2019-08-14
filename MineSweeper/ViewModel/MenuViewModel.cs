using System;
using System.Collections.Generic;
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


        /// <summary>
        /// Scores
        /// </summary>
        public int EasyScore { get; set; }

        public int IntermediateScore { get; set; }

        public int DifficultScore { get; set; }

        public int ScoreShowing { get; set; }


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

        /// <summary>
        /// Command to show the High Score menu
        /// </summary>
        public ICommand HighScoreCommand { get; set; }

        public ICommand ShowEasyHighScoreCommand { get; set; }
        public ICommand ShowMediumHighScoreCommand { get; set; }
        public ICommand ShowHardHighScoreCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MenuViewModel()
        {
            EasyScore = 20;
            IntermediateScore = 100;
            DifficultScore = 200;

            // Create commands
            StartEasyCommand = new RelayCommand(async () => await StartAsync(Difficulty.Easy));
            StartIntermediateCommand = new RelayCommand(async () => await StartAsync(Difficulty.Medium));
            StartDifficultCommand = new RelayCommand(async () => await StartAsync(Difficulty.Hard));

            NewGameCommand = new RelayCommand(NewGameButton);

            HideNewGameCommand = new RelayCommand(HideNewGame);

            ContinueCommand = new RelayCommand(ContinueButton);

            HighScoreCommand = new RelayCommand(HighScoreButton);

            ShowEasyHighScoreCommand = new RelayCommand(async () => await ShowHighScoreAsync(Difficulty.Easy));
            ShowMediumHighScoreCommand = new RelayCommand(async () => await ShowHighScoreAsync(Difficulty.Medium));
            ShowHardHighScoreCommand = new RelayCommand(async () => await ShowHighScoreAsync(Difficulty.Hard));

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


        public async Task ShowHighScoreAsync(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    ScoreShowing = EasyScore;
                    break;
                case Difficulty.Medium:
                    ScoreShowing = IntermediateScore;
                    break;
                case Difficulty.Hard:
                    ScoreShowing = DifficultScore;
                    break;
                default:
                    ScoreShowing = 123;
                    break;
            }

            await Task.Delay(1);
        }
        #endregion

        #region Helper Methods



        #endregion

    }
}
