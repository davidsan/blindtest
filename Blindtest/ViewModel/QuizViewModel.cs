﻿using Blindtest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Blindtest.ViewModel
{
    class QuizViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }
        private string selectedSong;

        public String SelectedSong
        {
            get { return selectedSong; }
            set { selectedSong = value; NotifyPropertyChanged("SelectedSong"); }
        }

        private ObservableCollection<String> songs;
        public ObservableCollection<String> Songs
        {
            get { return songs; }
            set { songs = value; NotifyPropertyChanged("Songs"); }
        }

        private int score;
        public int Score
        {
            get { return score; }
            set { score = value; NotifyPropertyChanged("Score"); }
        }

        private int roundsCount;
        public int RoundsCount
        {
            get { return roundsCount; }
            set { roundsCount = value; NotifyPropertyChanged("RoundsCount"); }
        }


        private ICommand btnSubmit;
        public ICommand BtnSubmit
        {
            get
            {
                return btnSubmit;
            }
            set
            {
                btnSubmit = value;
            }
        }

        // TODO Change this to Song type
        private String correctSong;
        public String CorrectSong
        {
            get { return correctSong; }
            set { correctSong = value; }
        }
        
        public QuizViewModel()
        {
            Songs = new ObservableCollection<string>();
            SelectedSong = null;
            Score = 0;
            RoundsCount = 0;
            BtnSubmit = new RelayCommand(new Action<object>(Submit));
            this.NewRound();
        }

        private void NewRound()
        {
            SelectedSong = null;
            // assign 4 new songs to the list + the correct one
            // TODO : wrap this in Quiz class
            Random rnd = new Random();
            Songs = new ObservableCollection<string>();
            for (int i = 0; i < 4; i++)
            {
                Songs.Add("Title " + rnd.Next(100) + " - Artist " + rnd.Next(100));
            }
            CorrectSong = Songs.Last();
        }

        public void Submit(object obj)
        {
            // increment rounds
            RoundsCount++;
            // check if selected song was correct, increment score if it was
            this.UpdateScore();
            this.NewRound();
        }

        private void UpdateScore()
        {
            if (CorrectSong == SelectedSong)
            {
                Score++;
            }
        }

    }
}
