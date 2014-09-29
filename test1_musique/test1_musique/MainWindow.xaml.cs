using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace test1_musique
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public class Entry
        {
            public String Title { get; set; }
            public String Link { get; set; }
        }

        String bonneRep;
        Random random = new Random();
        Entry[] feeds = new Entry[100]; //tableau contenant les titres et les liens de musiques
        RadioButton[] boutons = new RadioButton[4]; //tableau pour les components radioButton
        MediaPlayer mp = new MediaPlayer();
        Uri url = null;
        bool[] used = new bool[100]; // tableau pour savoir si une musique a deja ete joue
        bool choose = false;
        int nbRound = 1;
        int MAX_ROUND = 10;
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            XDocument myDoc = XDocument.Load(@"../../../../song.xml");

            boutons[0] = Radio1;
            boutons[1] = Radio2;
            boutons[2] = Radio3;
            boutons[3] = Radio4;
           
            String nsUrl = "http://www.w3.org/2005/Atom";

            feeds =
            (from entry in myDoc.Root.Elements(XName.Get("entry", nsUrl))
             select new Entry
             {
                 Title = entry.Element(XName.Get("title", nsUrl)).Value,
                 Link = entry.Elements(XName.Get("link", nsUrl)).Skip(1).First().Attribute("href").Value,
             }).ToArray<Entry>();

            
            for(int i = 0; i< feeds.Length; i++){
                used[i]=false;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!choose)
            {
                if ((bool)Radio1.IsChecked && (String)Radio1.Content == bonneRep)
                {
                    Reponse.Text = "Bonne reponse";
                    score++;
                }
                else if ((bool)Radio2.IsChecked && (String)Radio2.Content == bonneRep)
                {
                    Reponse.Text = "Bonne reponse";
                    score++;
                }
                else if ((bool)Radio3.IsChecked && (String)Radio3.Content == bonneRep)
                {
                    Reponse.Text = "Bonne reponse";
                    score++;
                }
                else if ((bool)Radio4.IsChecked && (String)Radio4.Content == bonneRep)
                {
                    Reponse.Text = "Bonne reponse";
                    score++;
                }
                else Reponse.Text = "Mauvaise reponse";
                choose = true;
                nextButton.Visibility = Visibility.Visible;
            }
            else
            {
                Reponse.Text = "Tu as déjà joué !!!";
            }
        }

        private void chooseSongAndPlay()
        {
            int r = random.Next(0, 100);
            int bonnechanson = random.Next(0, 4);


            for (int i = 0; i < boutons.Length; i++)
            {
                while (used[r] == true)
                {
                    r = random.Next(0, 100);
                }
                boutons[i].Content = feeds[r].Title;
                used[r] = true;
                if (i == bonnechanson)
                {
                    url = new Uri(feeds[r].Link);
                    bonneRep = feeds[r].Title;
                }
            }

            mp.Open(url);
            mp.Play();
        }

        private void startRound()
        {
            if (nbRound <= MAX_ROUND)
            {
                choose = false;
                Reponse.Text = "";
                roundLabel.Content = "Round " + nbRound;
                scoreLabel.Content = "Score : " + score;
                nextButton.Visibility = Visibility.Hidden;
                chooseSongAndPlay();
                nbRound++;
            }
            else
            {
                mp.Stop();
                gameCanvas.Visibility = Visibility.Hidden;
                endCanvas.Visibility = Visibility.Visible;
                scoreFinalLabel.Content = "Score Final : " + score; 
            }
        }

        private void startGame() 
        {
            score = 0;
            nbRound = 1;
            startRound();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            startCanvas.Visibility = Visibility.Hidden;
            gameCanvas.Visibility = Visibility.Visible;
            startGame();  
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            startRound();
        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            endCanvas.Visibility = Visibility.Hidden;
            gameCanvas.Visibility = Visibility.Visible;
            startGame();
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
