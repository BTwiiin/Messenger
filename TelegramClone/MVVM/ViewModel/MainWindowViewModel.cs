using System;
using System.Collections.ObjectModel;
using TelegramClient.MVVM.Core;
using TelegramClient.MVVM.Model;
using TelegramClient.Net;
using System.Linq;
using System.Windows;
using TelegramClone;

namespace TelegramClient.MVVM.ViewModel
{
    class MainWindowViewModel
    {
        public ObservableCollection<ClientModel> Clients { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public string Message { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        public RelayCommand ShutDownWindowCommand { get; set; }

        public string? Username { get; set; }

        private Server _server;
        public MainWindowViewModel() 
        {
            Clients = new ObservableCollection<ClientModel>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.disconnectionEvent += RemoveUser;
            _server.msgEvent += MessageReceived;
            _server.connectionEvent += UserConnected;
            SendMessageCommand = new RelayCommand(o => _server.SendMessage(Message), o => !string.IsNullOrEmpty(Message));
            ConnectToServerCommand = new RelayCommand(o =>  _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            ShutDownWindowCommand = new RelayCommand(o => _server.Close());
        }

        private void RemoveUser()
        {
            var uid = _server._packetReader.ReadMessage();
            var client = Clients.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Clients.Remove(client));
        }

        private void MessageReceived()
        {
            var message = _server._packetReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }

        private void UserConnected()
        {
            var client = new ClientModel
            {
                Username = _server._packetReader.ReadMessage(),
                UID = _server._packetReader.ReadMessage(),
            };

            if(!Clients.Any(x => x.UID == client.UID)) 
            {
                Application.Current.Dispatcher.Invoke(() => Clients.Add(client));
            }
        }
    }
}
