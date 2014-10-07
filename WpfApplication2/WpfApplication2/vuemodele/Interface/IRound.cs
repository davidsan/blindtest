using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.modele.classes;

namespace WpfApplication2.vuemodele.Interface
{
    interface IRound
    {
        public ISong[] Songs { get; set; }
        public ISong RightSong { get; set; }
    }
}
