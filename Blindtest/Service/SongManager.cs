using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Blindtest.Model;

namespace Blindtest.Service
{
    public class SongManager
    {
        // Singleton instance
        private static SongManager instance;
        private SongManager()
        {
            categorySongs = new List<Song>();
            selectedList = new List<Song>();
        }
        public static SongManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SongManager();
                }
                return instance;
            }
        }

        // iTunes US Top 100 Songs XML link
        private const String ITUNES_PREFIX_LINK = "https://itunes.apple.com/us/rss/topsongs/limit=100/";
        private const String ITUNES_CHARTS_XML_URL = ITUNES_PREFIX_LINK + "xml";
        private const String ITUNES_CHARTS_ELECTRONIC = ITUNES_PREFIX_LINK + "genre=7/xml";
        private const String ITUNES_CHARTS_DANCE = ITUNES_PREFIX_LINK + "genre=17/xml";
        private const String ITUNES_CHARTS_POP = ITUNES_PREFIX_LINK + "genre=14/xml";
        private const String ITUNES_CHARTS_ROCK = ITUNES_PREFIX_LINK + "genre=21/xml";
        private const String ITUNES_CHARTS_HIPHOP = ITUNES_PREFIX_LINK + "genre=18/xml";
        private const String ITUNES_CHARTS_FRENCH_POP = ITUNES_PREFIX_LINK + "genre=50000064/xml";
        private const String ITUNES_CHARTS_ALTERNATIVE = ITUNES_PREFIX_LINK + "genre=20/xml";
        private const String ITUNES_CHARTS_CLASSICAL = ITUNES_PREFIX_LINK + "genre=5/xml";
        private String myFile;

        private String oldCate = "";

        // Namespace used in iTunes XML
        static String NS_URL = "http://www.w3.org/2005/Atom";
        private List<Song> categorySongs;
        private List<Song> selectedList;
        public List<Song> Songs { get; set; }
        public List<Song> CategorySongs { get { return categorySongs; } set { categorySongs = value; } }
        public List<Song> SelectedList { get { return selectedList; } set { selectedList = value; } }
       

        public void FetchSongsItunes()
        {
            XDocument myDoc = XDocument.Load(myFile);
            Songs =
           (from entry in myDoc.Root.Elements(XName.Get("entry", NS_URL))
            select new Song
            {
                Title = entry.Element(XName.Get("title", NS_URL)).Value,
                Genre = entry.Element(XName.Get("category", NS_URL)).Attribute("term").Value,
                Link = entry.Elements(XName.Get("link", NS_URL)).Skip(1).First().Attribute("href").Value,
            }).ToList<Song>();
        }

        public void SelectCategoryList(String category)
        {
            if (!oldCate.Equals(category))
            {
                switch (category)
                {
                    case "All":
                        myFile = ITUNES_CHARTS_XML_URL;
                        break;
                    case "Electronic":
                        myFile = ITUNES_CHARTS_ELECTRONIC;
                        break;
                    case "Pop":
                        myFile = ITUNES_CHARTS_POP;
                        break;
                    case "Dance":
                        myFile = ITUNES_CHARTS_DANCE;
                        break;
                    case "Hip Hop/Rap":
                        myFile = ITUNES_CHARTS_HIPHOP;
                        break;
                    case "Rock":
                        myFile = ITUNES_CHARTS_ROCK;
                        break;
                    case "French Pop":
                        myFile = ITUNES_CHARTS_FRENCH_POP;
                        break;
                    case "Alternative":
                        myFile = ITUNES_CHARTS_ALTERNATIVE;
                        break;
                    case "Classical":
                        myFile = ITUNES_CHARTS_CLASSICAL;
                        break;
                    default:
                        myFile = ITUNES_CHARTS_XML_URL;
                        break;
                }
                oldCate = category;
                FetchSongsItunes();
            }
        }

    }
}