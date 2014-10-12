using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blindtest.Model
{
    class Quiz
    {

        public HashSet<Song> Songs { get; set; }

        public Song CorrectSong { get; set; }
        public Quiz(int nrofSongs)
        {
            Songs = new HashSet<Song>();
            SongManager sm = SongManager.Instance;
            Random rnd = new Random();
            while (Songs.Count < nrofSongs)
            {
                // draw a song from the song manager and add it to the set of songs
                Song s = sm.Songs[rnd.Next(sm.Songs.Count)];
                Songs.Add(s);
            }

            // pick the correct song among the set.
            CorrectSong = Songs.ElementAt(rnd.Next(Songs.Count));
        }



    }
}
