using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2.vuemodele.Interface
{
    interface ISong
    {
        public string Link { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
