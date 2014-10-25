﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blindtest.View;
using System.Windows.Input;
using Blindtest.Service;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Blindtest.ViewModel
{
    class OptionViewModel : ViewModelBase
    {
        #region Constructor 
        private static OptionViewModel instance;
        public static OptionViewModel Instance
        {
            get
            {
                if (instance == null) { instance = new OptionViewModel(); }
                return instance;
            }
        }

        public OptionViewModel()
        {
            CategorieList = new ObservableCollection<string>();
            BtnSettings = new RelayCommand(new Action<object>(Settings));
            BtnBack = new RelayCommand(new Action<object>(Back));
            Categorie = "All";
            CategorieList.Add("All");
            CategorieList.Add("Pop");
            CategorieList.Add("Rock");
            CategorieList.Add("Dance");
            CategorieList.Add("Classical");
            CategorieList.Add("Electronic");
            CategorieList.Add("Hip Hop/Rap");
            CategorieList.Add("Alternative");
            CategorieList.Add("French Pop");
            Volume = 50;
        }
        #endregion // Constructor

        #region Field
        private NetworkManager nm = NetworkManager.Instance;
        private ViewModelBase previousViewModel;
        private UserControl previousView;
        private ICommand btnSettings;
        private ICommand btnBack;
        private ObservableCollection<String> categorieList;
        private String categorie;
        private int volume;
        #endregion // Field

        #region Properties / Command
        public ViewModelBase PreviousViewModel
        {
            get { return previousViewModel; }
            set { previousViewModel = value; }
        }

        public UserControl PreviousView
        {
            get { return previousView; }
            set { previousView = value; }
        }
        
        public ICommand BtnSettings
        {
            get { return btnSettings; }
            set { btnSettings = value; }
        }

        public ICommand BtnBack
        {
            get { return btnBack; }
            set { btnBack = value; }
        }

        public ObservableCollection<String> CategorieList
        {
            get { return categorieList; }
            set { categorieList = value; }
        }

        public String Categorie
        {
            get { return categorie; }
            set { categorie = value; OnPropertyChanged("Categorie"); }
        }

        public int Volume
        {
            get { return volume; }
            set { volume = value; }
        }
        #endregion // Properties / Command

        #region Action / Function
        private void Settings(object obj)
        {
            // Pour le moment rien
        }

        private void Back(object obj)
        {
            if (!nm.IsInGame)
            {
                if (nm.IsOnline)
                {
                    String connectStr = "category;" + Categorie + ";";
                    byte[] reponseByServer = ASCIIEncoding.ASCII.GetBytes(connectStr.ToString());
                    nm.Sock.Send(reponseByServer);
                    nm.Category = Categorie;
                }
                else
                {
                    nm.Category = Categorie;
                }
            }
            MainWindow.Instance.contentControl.Content = PreviousView;
            MainWindow.Instance.DataContext = PreviousViewModel;
            
        }
        #endregion // Action / Function
    }
}
