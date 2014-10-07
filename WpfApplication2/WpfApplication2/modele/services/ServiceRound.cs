using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.modele.classes;

namespace WpfApplication2.modele.services
{
    /// <summary>
    /// On choisi la bonne chanson
    /// </summary>
    class ServiceRound
    {
        Random random = new Random();
        public Song GetRightSong(Song[] s)
        {
           int r = random.Next(0, s.Length);
           return s[r];
        }
    }
}
