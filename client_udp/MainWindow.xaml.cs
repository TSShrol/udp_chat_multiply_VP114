using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace client_udp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string serverIP = "127.0.0.1";
        const short serverPort = 8007;
        
        bool isListening = false;
        IPEndPoint serverEndPoint;
        UdpClient client = new UdpClient();
        public MainWindow()
        {
            InitializeComponent();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP),serverPort);
        }

        private async void SendMessage(string message) {
            byte[] data = Encoding.Unicode.GetBytes(message);
            if(isListening)
                await client.SendAsync(data, data.Length,serverEndPoint);
        }

        private async void Listen() {
            while (isListening) {
                try
                {
                    var resultData = await client.ReceiveAsync();
                    string message = Encoding.Unicode.GetString(resultData.Buffer);
                    chatList.Items.Add(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
            
                }
            }
        }

        private void joinItem_Click(object sender, RoutedEventArgs e)
        {
         if(!isListening)
            isListening = true;
            SendMessage("<|join|>");
            Listen();
            
        }

        private void quitItem_Click(object sender, RoutedEventArgs e)
        {
            isListening = false;
            SendMessage("<|quit|>");

        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            string message = msgTextBox.Text;
            if (message != "") {
                SendMessage(message);
            }
        }
    }
}
