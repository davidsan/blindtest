using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.modele.classes;

namespace WpfApplication2.modele.services
{
    class ServiceGame2
    {
        Game game = new Game();
        
        public void Init()
        {
            game.Score = 0;
            game.NumRound = 1;
        }
    }
}
