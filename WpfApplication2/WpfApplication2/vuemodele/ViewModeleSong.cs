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
    /// <summary>
    /// Liaison entre la vue et le modèle
    /// </summary>
    /// 
    class ViewModeleSong : ObservableObject , ISong
    {
        public string _name = null;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string _link = null;
        public string Link
        {
            get { return _link; }
            set
            {
                _name = value;
                OnPropertyChanged("Link");
            }
        }

        public string _category = null;
        public string Category
        {
            get { return _category; }
            set
            {
                _name = value;
                OnPropertyChanged("Category");
            }
        }
    }
}
