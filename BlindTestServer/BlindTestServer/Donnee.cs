using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlindTestServer;

namespace BlindTestServer
{
    class Donnee
    {
        private List<String> userList;
        private String xmlUrl = "https://itunes.apple.com/us/rss/topsongs/limit=100/xml";

        public Donnee()
        {
            userList = new List<string>();
        }

        public bool exist(String name) {
            return userList.Exists(x => x.Equals(name));
        }

        public void addUser(String name)
        {
            userList.Add(name);
        }

        public void removeUser(String name)
        {
            userList.Remove(name);
        }

        public String getXmlUrl()
        {
            return xmlUrl;
        }
    }
}
