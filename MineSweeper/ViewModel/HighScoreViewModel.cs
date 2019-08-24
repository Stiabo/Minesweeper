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

        public bool EasyScoreOpen { get; set; } = true;
        public bool MediumScoreOpen { get; set; }
        public bool HardScoreOpen { get; set; }


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
                HighScoreViewModel JSONHighScoreData = new HighScoreViewModel();
                string output = File.ReadAllText(InstancePathHighScore);

                //Deserialize from JSON to .NET and put into HighScoreData
                JSONHighScoreData = JsonConvert.DeserializeObject<HighScoreViewModel>(output);

                EasyScore = JSONHighScoreData.EasyScore;
                IntermediateScore = JSONHighScoreData.IntermediateScore;
                DifficultScore = JSONHighScoreData.DifficultScore;
                ScoreShowing = EasyScore;

            }

        }

        /// <summary>
        /// Stores the all data on GameInstance to a JSON file
        /// </summary>
        public void StoreHighScoreData()
        {
            //Convert a copy of current instance to Json format
            string output = JsonConvert.SerializeObject(MenuViewModel.HighScoreInstance);

            //Save output to file
            //string path = "..\\..\\JSON\\GameInstance.json";
            if (File.Exists(InstancePathHighScore)) File.WriteAllText(InstancePathHighScore, output);
        }

        /// <summary>
        /// Set all the tabs on the High Score menu to false
        /// </summary>
        public void CloseAllScoreTabs()
        {
            EasyScoreOpen = false;
            MediumScoreOpen = false;
            HardScoreOpen = false;
        }

        #endregion


        public async Task ShowHighScoreAsync(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    ScoreShowing = EasyScore;
                    CloseAllScoreTabs();
                    EasyScoreOpen = true;
                    break;
                case Difficulty.Medium:
                    ScoreShowing = IntermediateScore;
                    CloseAllScoreTabs();
                    MediumScoreOpen = true;
                    break;
                case Difficulty.Hard:
                    ScoreShowing = DifficultScore;
                    CloseAllScoreTabs();
                    HardScoreOpen = true;
                    break;
                default:                    
                    break;
            }

            await Task.Delay(1);
        }

        
    }
}
