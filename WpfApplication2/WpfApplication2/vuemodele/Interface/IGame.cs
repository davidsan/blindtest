using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.modele.classes;

namespace WpfApplication2.vuemodele.Interface
{
    interface IGame
    {
        public Round[] Rounds { get; set; }
        public int NumRound { get; set; }
        public int Score { get; set; }
    }
    }
}
