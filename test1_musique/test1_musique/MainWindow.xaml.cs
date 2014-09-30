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
            public String Category { get; set; }
        }

        String bonneRep;
        String genreMusic;
        Random random = new Random();
        Entry[] feeds = new Entry[200]; //tableau contenant les titres et les liens de musiques
        Entry[] genre = new Entry[100];
        Entry[] selectTab = null;
        RadioButton[] boutons = new RadioButton[4]; //tableau pour les components radioButton
        MediaPlayer mp = new MediaPlayer();
        Uri url = null;
        bool[] used = new bool[200]; // tableau pour savoir si une musique a deja ete joue
        bool[] selected = new bool[200];
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
                 Category = entry.Element(XName.Get("category", nsUrl)).Attribute("term").Value,
                 Link = entry.Elements(XName.Get("link", nsUrl)).Skip(1).First().Attribute("href").Value,
             }).ToArray<Entry>();

            for(int i = 0; i< feeds.Length; i++){
                selected[i] = false;
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
                nextButton.IsEnabled = true;
            }
            else
            {
                Reponse.Text = "Tu as déjà joué !!!";
            }
        }

        private void chooseSongAndPlay()
        {
            if (selectTab == null) selectTab = feeds;
            int max_range = selectTab.Count();
            int r;
            int bonnechanson = random.Next(0, 4);
            for (int i = 0; i < selectTab.Length; i++)
            {
                used[i] = false;
            }

            for (int i = 0; i < boutons.Length; i++)
            {
                r = random.Next(0, max_range);
                while (used[r] == true || (selected[r] == true && i == bonnechanson))
                {
                    r = random.Next(0, max_range);
                }
                boutons[i].Content = selectTab[r].Title;
                used[r] = true;
                if (i == bonnechanson)
                {
                    url = new Uri(selectTab[r].Link);
                    bonneRep = selectTab[r].Title;
                    selected[r] = true;
                }
            }

            mp.Open(url);
            mp.Play();
        }

        private void startRound()
        {
            foreach (RadioButton bu in boutons)
            {
                bu.IsChecked = false;
            }
            if (nbRound <= MAX_ROUND)
            {
                choose = false;
                Reponse.Text = "";
                roundLabel.Content = "Round " + nbRound;
                scoreLabel.Content = "Score : " + score;
                nextButton.IsEnabled = false;
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
            genreMusic = genreCombo.SelectionBoxItem.ToString();
            Console.WriteLine(genreMusic);
            if (genreMusic.Equals("All"))
            {
                selectTab = feeds;
            }
            else
            {
                List<Entry> tmp = new List<Entry>();
                foreach (Entry en in feeds)
                {
                    if (en.Category.Equals(genreMusic))
                    {
                        tmp.Add(en);
                    }
                }
                genre = tmp.ToArray();
                selectTab = genre;
            }
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

        private void menuButton_Click(object sender, RoutedEventArgs e)
        {
            endCanvas.Visibility = Visibility.Hidden;
            startCanvas.Visibility = Visibility.Visible;
        }

        private void mainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            mp.Stop();
            for (int i = 0; i < feeds.Length; i++)
            {
                selected[i] = false;
            }
            gameCanvas.Visibility = Visibility.Hidden;
            startCanvas.Visibility = Visibility.Visible;
        }

    }
}
