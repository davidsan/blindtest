using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Blindtest.Service;
namespace Blindtest.Audio
{
    class AudioManager
    {
        // Singleton instance
        private NetworkManager nm = NetworkManager.Instance;
        private static AudioManager instance;

        public MediaPlayer Player { get; set; }

        private AudioManager()
        {
            Player = new MediaPlayer();
            Player.MediaEnded += Media_Ended;
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

        public void Play(String uri)
        {
            Player.Open(new Uri(uri));
            Player.Play();
        }

        public void Stop()
        {
            Player.Stop();
        }

        public void setSound(int value)
        {
            Player.Volume = value;
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            if (nm.IsOnline)
            {
                String connectStr = "timesup;";
                MessageManager.sendMessageToServer(connectStr);
            }
        }
    }
}
