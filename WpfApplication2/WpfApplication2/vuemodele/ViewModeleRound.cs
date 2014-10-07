using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq.Expressions;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using WpfApplication2.modele.classes;
using WpfApplication2.modele.services;
using WpfApplication2.vuemodele.Design;
using WpfApplication2.vuemodele.Interface;
using System.ComponentModel;
using WpfApplication2.helpers;


namespace WpfApplication2.vuemodele
{
    class ViewModeleRound : ObservableObject, IRound
    {

        this.Data

        public Song[] _songs = null;
        public Song[] Songs
        {
            get { return _songs; }
            set
            {
                _songs = value;
                OnPropertyChanged("Songs");
            }
        }

        public Song _rightSong = null;
        public Song RightSong
        {
            get { return _rightSong; }
            set
            {
                _rightSong = value;
                OnPropertyChanged("RightSong");
            }
        }

        public Song _Song1 = null;
        public Song Song1
        {
            get { return _Song1; }
            set
            {
                _Song1 = value;
                OnPropertyChanged("Song1");
            }
        }

        public Song _Song2 = null;
        public Song Song2
        {
            get { return _Song2; }
            set
            {
                _Song2 = value;
                OnPropertyChanged("Song2");
            }
        }

        public Song _Song3 = null;
        public Song Song3
        {
            get { return _Song3; }
            set
            {
                _Song3 = value;
                OnPropertyChanged("Song3");
            }
        }

        public Song _Song4 = null;
        public Song Song4
        {
            get { return _Song4; }
            set
            {
                _Song4 = value;
                OnPropertyChanged("Song4");
            }
        }


        public ICommand GetGoodSongCommand { get; private set; }
        public ICommand ValiderCommand { get; private set; }

        public ViewModeleRound()
        {
            GetGoodSongCommand = new RelayCommand(GetGoodSong);
        }


        //methode pour récuperer la bonne chanson depuis le modele(serviceRound)
        public void GetGoodSong()
        {
            //services round et song
            ServiceRound serviceRound = new ServiceRound();
            ServiceSong serviceSong = new ServiceSong();
            Round round = new Round();
            round.Songs = serviceSong.ChooseSongs(4);
            round.RightSong = serviceRound.GetRightSong(round.Songs);
            round.Song1 = round.Songs[0];
            round.Song2 = round.Songs[1];
            round.Song3 = round.Songs[2];
            round.Song4 = round.Songs[3];


            this.Songs = round.Songs;
            this.RightSong = round.RightSong;
            this.Song1 = round.Song1;
            this.Song2 = round.Song2;
            this.Song3 = round.Song3;
            this.Song4 = round.Song4;
        }

        public void Valider(){


        }
    }
}
