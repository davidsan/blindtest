using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BlindTestServer.Model;

namespace BlindTestServer.Service
{
    class SongManager
    {
        // Singleton instance
        private static SongManager instance;
        private SongManager()
        {
            this.FetchSongsItunes();
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
        static String ITUNES_CHARTS_XML_URL = "https://itunes.apple.com/us/rss/topsongs/limit=100/xml";
        // Namespace used in iTunes XML
        static String NS_URL = "http://www.w3.org/2005/Atom";
        public List<Song> Songs { get; set; }

        private void FetchSongsItunes()
        {
            XDocument myDoc = XDocument.Load(ITUNES_CHARTS_XML_URL);
            Songs =
           (from entry in myDoc.Root.Elements(XName.Get("entry", NS_URL))
            select new Song
            {
                Title = entry.Element(XName.Get("title", NS_URL)).Value,
                Genre = entry.Element(XName.Get("category", NS_URL)).Attribute("term").Value,
                Link = entry.Elements(XName.Get("link", NS_URL)).Skip(1).First().Attribute("href").Value,
            }).ToList<Song>();
        }
    }
}
