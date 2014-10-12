using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Blindtest.Audio
{
    class AudioManager
    {
        // Singleton instance
        private static AudioManager instance;
        public MediaPlayer Player { get; set; }
        private AudioManager()
        {
            Player = new MediaPlayer();
        }
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                }
                return instance;
            }
        }

        internal void Play(String uri)
        {
            Player.Open(new Uri(uri));
            Player.Play();
        }
        internal void Stop()
        {
            Player.Stop();
        }
    }
}
