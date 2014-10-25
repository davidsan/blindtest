﻿using Blindtest.Model;
using Blindtest.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Blindtest.ViewModel
{
    class QuizViewModel : ViewModelBase
    {

        #region Fields

        private string selectedSong;
        private ObservableCollection<String> songs;
        private int score;
        private int roundsCount;
        private String lastAnswer;
        private ICommand btnSubmit;
        private ICommand btnSettings;
        private String correctSong; // TODO Change this to Song type

        #endregion // Fields

        #region Public Properties / Commands

        public String SelectedSong
        {
            get { return selectedSong; }
            set { selectedSong = value; OnPropertyChanged("SelectedSong"); }
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

        public ICommand BtnSubmit
        {
            get { return btnSubmit; }
            set { btnSubmit = value; }
        }

        public String CorrectSong
        {
            get { return correctSong; }
            set { correctSong = value; }
        }

        public ICommand BtnSettings
        {
            get { return btnSettings; }
            set { btnSettings = value; }
        }

        #endregion // Public Properties / Commands

        #region Contructor
        public QuizViewModel()
        {
            Songs = new ObservableCollection<string>();
            SelectedSong = null;
            Score = 0;
            RoundsCount = 1;
            LastAnswer = "You have 30 seconds to find out the artist and the title of the song you hear";
            BtnSubmit = new RelayCommand(new Action<object>(Submit));
            BtnSettings = new RelayCommand(new Action<object>(Settings));
        }
        #endregion // Constructor

        #region Action / Function
        public void Play()
        {
            this.NewRound();
        }

        private void NewRound()
        {
            SelectedSong = null;
            Songs = new ObservableCollection<string>();
            Quiz q = new Quiz(4);
            for (int i = 0; i < 4; i++)
            {
                Songs.Add(q.Songs.ElementAt(i).Title);
            }
            if (RoundsCount > 1)
            {
                LastAnswer = "The answer was " + CorrectSong;
            }
            CorrectSong = q.CorrectSong.Title;
            Audio.AudioManager.Instance.Play(q.CorrectSong.Link);
        }

        private void Submit(object obj)
        {
            Audio.AudioManager.Instance.Stop();
            // increment rounds
            RoundsCount++;
            // check if selected song was correct, increment score if it was
            this.UpdateScore();
            if (RoundsCount > 6)
            {
                ResultViewModel rvm = new ResultViewModel(Score.ToString());
                rvm.IsOnline = false;
                MainWindow.Instance.contentControl.Content = new ResultView();
                MainWindow.Instance.DataContext = rvm;
                return;
            }
            this.NewRound();

        }

        private void UpdateScore()
        {
            if (CorrectSong == SelectedSong)
            {
                Score++;
            }
        }

        private void Settings(object obj)
        {
            OptionViewModel opt = OptionViewModel.Instance;
            opt.PreviousView = MainWindow.Instance.contentControl.Content as UserControl;
            opt.PreviousViewModel = this;
            MainWindow.Instance.DataContext = opt;
            MainWindow.Instance.contentControl.Content = new OptionView();
        }
        #endregion // Action / Function

    }
}
