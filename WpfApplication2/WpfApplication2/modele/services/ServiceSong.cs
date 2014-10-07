using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.modele.classes;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace WpfApplication2.modele.services
{

    /// <summary>
    /// on récupère toutes les chansons dans un tableau
    /// </summary>
    class ServiceSong
    {

        static Song[] feeds = new Song[200]; //tableau contenant les titres et les liens de musiques
        bool[] used = new bool[200]; // tableau pour savoir si une musique a deja ete joue
        bool[] selected = new bool[200];
        static Song[] selectTab = null;
        String bonnechanson;
        String genreMusic;
        Random random = new Random();
        Round round = new Round();


        public Song[] ChooseSongs(int nbSongs)//prend nombre de chansons à garder + liste des chansons en entrée
        {
            Song[] ChoosedSongs = null;
            if (selectTab == null) selectTab = feeds;
            int max_range = selectTab.Count();
            int r;
          
            for (int i = 0; i < selectTab.Length; i++)
            {
                used[i] = false;
            }

            for (int i = 0; i < nbSongs; i++)
            {
                r = random.Next(0, max_range);
                while (used[r] == true || selected[r] == true)
                {
                    r = random.Next(0, max_range);
                }
                ChoosedSongs[r] = selectTab[r];
                used[r] = true;
            }
            return ChoosedSongs;
        }

        static Song[] LoadSongs() 
        {
            XDocument myDoc = XDocument.Load(@"C:\Users\Floppy\Documents\David\M2\TPGP\blindtest\song.xml");
            String nsUrl = "http://www.w3.org/2005/Atom";

            feeds =
            (from entry in myDoc.Root.Elements(XName.Get("entry", nsUrl))
             select new Song
             {
                 Name = entry.Element(XName.Get("title", nsUrl)).Value,
                 Category = entry.Element(XName.Get("category", nsUrl)).Attribute("term").Value,
                 Link = entry.Elements(XName.Get("link", nsUrl)).Skip(1).First().Attribute("href").Value,
             }).ToArray<Song>();

            return feeds;
           
        }
    }
}
