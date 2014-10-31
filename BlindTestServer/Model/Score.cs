using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlindTestServer.Model
{
    class Score
    {
        
        [Key]
        public string Name { get; set; }
        public int Points { get; set; }
    }
}
