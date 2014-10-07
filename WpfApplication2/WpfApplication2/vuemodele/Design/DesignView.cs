using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.vuemodele.Interface;
using WpfApplication2.modele.classes;

namespace WpfApplication2.vuemodele.Design
{

    /// <summary>
    /// chansons tests qui vont servir juste pour le mode design
    /// </summary>
    class DesignView : ISong, IRound, IGame
    {

        Song s1 = new Song("t", "t", "t");

    }
}
