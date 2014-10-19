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

namespace Blindtest.ViewModel
{
    class QuizOnlineViewModel : ViewModelBase
    {

        NetworkManager nm = NetworkManager.Instance;
        private string selectedSong;
        private ObservableCollection<String> songs;
        private int score;
        private int roundsCount;
        private String correctSongurl;
        private String correctSongTitre;
        private String lastAnswer;
        private ICommand btnSubmitOnline;

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


        public QuizOnlineViewModel()
        {
            Songs = new ObservableCollection<string>();
            SelectedSong = null;
            Score = 0;
            BtnSubmitOnline = new RelayCommand(new Action<object>(SubmitOnline));
        }

        public void Play()
        {
            this.NewRound();
        }

        private void NewRound()
        {
            SelectedSong = null;
            Audio.AudioManager.Instance.Play(CorrectSongUrl);
        }

        private void SubmitOnline(object obj)
        {
            String connectStr = "connect;" + SelectedSong + ";\n";
            byte[] reponseByServer = ASCIIEncoding.ASCII.GetBytes(connectStr.ToString());
            nm.Sock.Send(reponseByServer);

            Audio.AudioManager.Instance.Stop();
            if (RoundsCount > 6)
            {
                MainWindow.Instance.contentControl.Content = new ResultView();
                MainWindow.Instance.DataContext = new ResultViewModel(score);
                return;
            }
            Songs.Clear();
            this.NewRound();

        }

    }
}
