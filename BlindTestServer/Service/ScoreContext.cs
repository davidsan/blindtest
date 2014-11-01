using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BlindTestServer.Model;

namespace BlindTestServer.Service
{
    class ScoreContext : DbContext
    {
        public DbSet<Score> DbSetScores { get; set; }
    }
}
