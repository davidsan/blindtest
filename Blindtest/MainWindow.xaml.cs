using Blindtest.View;
using Blindtest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.ComponentModel;
using Blindtest.Service;
using System.Net.Sockets;

namespace Blindtest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Instance { get; set; }

        private NetworkManager nm = NetworkManager.Instance;

        static MainWindow()
        {
            Instance = new MainWindow();

        }
        private MainWindow()
        {
            InitializeComponent();
            this.DataContext = new LandingViewModel();
            this.contentControl.Content = new LandingView();
        }

        void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            if (nm.IsOnline)
            {
                String connectStr = "exit;";
                MessageManager.sendMessageToServer(connectStr);
                nm.Sock.Shutdown(SocketShutdown.Both);
                nm.Sock.Close();
            }
        }

    }
}
