using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2.modele.classes
{

    /// <summary>
    /// classe Song simple avec propriétés get/set
    /// représente une chanson avec un titre, un lien et une catégorie
    /// </summary>
    class Song
    {
        public string Link { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
