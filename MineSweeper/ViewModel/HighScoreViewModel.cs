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
    public class HighScoreViewModel : BaseViewModel
    {
        #region Public Properties

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

        public static string InstancePathHighScore { get; } = "..\\..\\JSON\\HighScore.json";

        public int ScoreShowing { get; set; }


        //HighScoreInstance
        //public static HighScoreViewModel HighScoreInstance => new HighScoreViewModel();

        #endregion


        public ICommand ShowEasyHighScoreCommand { get; set; }
        public ICommand ShowMediumHighScoreCommand { get; set; }
        public ICommand ShowHardHighScoreCommand { get; set; }


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public HighScoreViewModel()
        {
            EasyScore = 10;
            IntermediateScore = 20;
            DifficultScore = 30;

            ScoreShowing = 123;


            ShowEasyHighScoreCommand = new RelayCommand(async () => await ShowHighScoreAsync(Difficulty.Easy));

            ShowMediumHighScoreCommand = new RelayCommand(async () => await ShowHighScoreAsync(Difficulty.Medium));

            ShowHardHighScoreCommand = new RelayCommand(async () => await ShowHighScoreAsync(Difficulty.Hard));

        }

        #endregion


        #region Helper Methods

        /// <summary>
        /// Restores GameInstance from a JSON file
        /// </summary>
        public void RestoreHighScoreData()
        {
            //Get data from file
            //string path = "..\\..\\JSON\\GameInstance.json";
            if (File.Exists(InstancePathHighScore))
            {
                string output = File.ReadAllText(InstancePathHighScore);

                //Deserialize from JSON to .NET and put into HighScoreInstance
                //HighScoreInstance = JsonConvert.DeserializeObject<HighScoreViewModel>(output);

            }

        }

        #endregion


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
                    break;
            }

            await Task.Delay(1);
        }

    }
}
