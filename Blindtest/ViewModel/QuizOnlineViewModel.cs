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
using System.Collections.ObjectModel;

namespace Blindtest.ViewModel
{
    class QuizOnlineViewModel : ViewModelBase, INotifyPropertyChanged
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
            set { selectedSong = value; RaisePropertyChanged("SelectedSong"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public String CorrectSongUrl
        {
            get { return correctSongurl; }
            set { correctSongurl = value; }
        }

        public ObservableCollection<String> Songs
        {
            get { return songs; }
            set { songs = value; RaisePropertyChanged("Songs"); }
        }

        public int Score
        {
            get { return score; }
            set { score = value; RaisePropertyChanged("Score"); }
        }

        public int RoundsCount
        {
            get { return roundsCount; }
            set { roundsCount = value; RaisePropertyChanged("RoundsCount"); }
        }

        public String LastAnswer
        {
            get { return lastAnswer; }
            set { lastAnswer = value; RaisePropertyChanged("LastAnswer"); }
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
            LastAnswer = "You have 30 seconds to find out the artist and the title of the song you hear";
            WaitNextRound = false;
            BtnSubmitOnline = new RelayCommand(new Action<object>(SubmitOnline), x => !WaitNextRound);
        }

        public void Play()
        {
            this.WaitNextRound = false;
            this.NewRound();
        }

        private void NewRound()
        {
            if (RoundsCount > 1)
            {
                LastAnswer = "The answer was " + CorrectSongTitre;
            }
            SelectedSong = null;
            Audio.AudioManager.Instance.Play(CorrectSongUrl);
        }

        private void SubmitOnline(object obj)
        {
            String connectStr = "guess;" + SelectedSong + ";\n";
            byte[] reponseByServer = ASCIIEncoding.ASCII.GetBytes(connectStr.ToString());
            nm.Sock.Send(reponseByServer);

            Audio.AudioManager.Instance.Stop();
            WaitNextRound = true;
        }

        private bool wait;
        public bool WaitNextRound
        {
            get { return wait; }
            set { 
                wait = value;
                RaisePropertyChanged("WaitNextRound");
            }
        }

    }
}
