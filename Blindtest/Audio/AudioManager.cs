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

        /// <summary>
        /// Demarre une chanson
        /// </summary>
        /// <param name="uri"></param>
        public void Play(String uri)
        {
            Player.Open(new Uri(uri));
            Player.Play();
        }

        /// <summary>
        /// Arrete une chanson
        /// </summary>
        public void Stop()
        {
            Player.Stop();
        }

        /// <summary>
        /// Reglage du volume
        /// </summary>
        /// <param name="value"></param>
        public void setSound(int value)
        {
            Player.Volume = value;
        }

        /// <summary>
        /// Envoi au serveur la fin de la chanson pour le client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
