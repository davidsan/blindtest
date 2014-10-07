using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.modele.classes;

namespace WpfApplication2.modele.classes
{
    class Round
    {
        public Song[] Songs{get; set;} //estce q'on en a besoin ?

        public Song Song1 { get; set; }
        public Song Song2 { get; set; }
        public Song Song3 { get; set; }
        public Song Song4 { get; set; }

        public Song RightSong { get; set; }
    }
}
