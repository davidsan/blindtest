using Blindtest.Model;
using Blindtest.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Blindtest.Service;
using System.Threading;
using System.Windows.Controls;
using Blindtest.Audio;

namespace Blindtest.ViewModel
{
    class QuizOnlineViewModel : ViewModelBase
    {
        #region Contructor
        public QuizOnlineViewModel()
        {
            Songs = new ObservableCollection<string>();
            SelectedSong = null;
            Score = 0;
            LastAnswer = "You have 30 seconds to find out the artist and the title of the song you hear";
            WaitNextRound = false;
            BtnSubmitOnline = new RelayCommand(new Action<object>(SubmitOnline), PredicateOnline);
            BtnSettings = new RelayCommand(new Action<object>(Settings));
        }
        #endregion // Contructor

        #region Field
        NetworkManager nm = NetworkManager.Instance;
        private string selectedSong;
        private ObservableCollection<String> songs;
        private int score;
        private int roundsCount;
        private String correctSongurl;
        private String correctSongTitre;
        private String lastAnswer;
        private bool wait;
        private ICommand btnSubmitOnline;
        private ICommand btnSettings;
        #endregion Field

        #region Properties / Command
        private bool PredicateOnline(object obj)
        {
            return !WaitNextRound;
        }

        public String CorrectSongTitre
        {
            get { return correctSongTitre; }
            set { correctSongTitre = value; }
        }

        public String SelectedSong
        {
            get { return selectedSong; }
            set { selectedSong = value; OnPropertyChanged("SelectedSong"); }
        }

        public String CorrectSongUrl
        {
            get { return correctSongurl; }
            set { correctSongurl = value; }
        }

        public ObservableCollection<String> Songs
        {
            get { return songs; }
            set { songs = value; OnPropertyChanged("Songs"); }
        }

        public int Score
        {
            get { return score; }
            set { score = value; OnPropertyChanged("Score"); }
        }

        public int RoundsCount
        {
            get { return roundsCount; }
            set { roundsCount = value; OnPropertyChanged("RoundsCount"); }
        }

        public String LastAnswer
        {
            get { return lastAnswer; }
            set { lastAnswer = value; OnPropertyChanged("LastAnswer"); }
        }

        public ICommand BtnSubmitOnline
        {
            get { return btnSubmitOnline; }
            set { btnSubmitOnline = value; }
        }

        public ICommand BtnSettings
        {
            get { return btnSettings; }
            set { btnSettings = value; }
        }

        public bool WaitNextRound
        {
            get { return wait; }
            set
            {
                wait = value; OnPropertyChanged("WaitNextRound");
            }
        }
       
        #endregion // Properties / Command

        #region Action / Function
        /// <summary>
        /// Demarre un nouveau round
        /// </summary>
        public void Play()
        {
            this.WaitNextRound = false;
            this.NewRound();
        }

        /// <summary>
        /// Demarre la nouvelle musique
        /// </summary>
        private void NewRound()
        {
            if (RoundsCount > 1)
            {
                LastAnswer = "The answer was " + CorrectSongTitre;
            }
            SelectedSong = null;
            AudioManager.Instance.Play(CorrectSongUrl);
        }

        /// <summary>
        /// Envoi la reponse choisi pour le round au serveur
        /// </summary>
        /// <param name="obj"></param>
        private void SubmitOnline(object obj)
        {
            String connectStr = "guess;" + SelectedSong + ";\n";
            MessageManager.sendMessageToServer(connectStr);
            Audio.AudioManager.Instance.Stop();
            WaitNextRound = true;
        }

        /// <summary>
        /// Permet d'afficher la fenetre d'option
        /// </summary>
        /// <param name="obj"></param>
        private void Settings(object obj)
        {
            OptionViewModel opt = OptionViewModel.Instance;
            opt.PreviousView = MainWindow.Instance.contentControl.Content as UserControl;
            opt.PreviousViewModel = this;
            MainWindow.Instance.DataContext = opt;
            MainWindow.Instance.contentControl.Content = new OptionView();
        }

        #endregion

    }
}
