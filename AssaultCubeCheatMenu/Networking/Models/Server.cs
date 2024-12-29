using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AssaultCubeCheatMenu.Networking
{
    public class Server
    {

        public string Name { get; set; }
        public int PingMs { get; set; }
        public List<string> Players = new List<string>();
        private TcpListener _listener;
        public List<ClientHandler> _clients;
      
        private Thread _acceptThread;

        public event Action<string> OnClientMessage;

        public void Start(int port)
        {
            _clients = new List<ClientHandler>();
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();

            _acceptThread = new Thread(AcceptClients) { IsBackground = true };
            _acceptThread.Start();
        }

        public void Stop()
        {
            foreach (var client in _clients)
            {
                client.Disconnect();
            }
            _listener?.Stop();
            _acceptThread?.Abort();
        }

        public void BroadcastMessage(string message)
        {
            foreach (var client in _clients)
            {
                client.SendMessage(message);
            }
        }

        private void AcceptClients()
        {
            while (true)
            {
                try
                {
                    TcpClient tcpClient = _listener.AcceptTcpClient();
                    ClientHandler clientHandler = new ClientHandler(tcpClient);
                    clientHandler.OnMessageReceived += HandleClientMessage;
                    _clients.Add(clientHandler);
                    Console.WriteLine("Client connected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
                    break;
                }
            }
        }

        private void HandleClientMessage(ClientHandler client, string message)
        {
            OnClientMessage?.Invoke(message);
            BroadcastMessage(message); // Echo to all clients
        }

        public class ClientHandler
        {
            private TcpClient _tcpClient;
            private StreamReader _reader;
            private StreamWriter _writer;

            public event Action<ClientHandler, string> OnMessageReceived;

            public ClientHandler(TcpClient tcpClient)
            {
                _tcpClient = tcpClient;
                NetworkStream stream = _tcpClient.GetStream();
                _reader = new StreamReader(stream, Encoding.UTF8);
                _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                Thread receiveThread = new Thread(ReceiveMessages) { IsBackground = true };
                receiveThread.Start();
            }

            public void SendMessage(string message)
            {
                _writer.WriteLine(message);
            }

            public void Disconnect()
            {
                _tcpClient.Close();
            }

            private void ReceiveMessages()
            {
                try
                {
                    while (_tcpClient.Connected)
                    {
                        string message = _reader.ReadLine();
                        if (message != null)
                        {
                            OnMessageReceived?.Invoke(this, message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in client handler: {ex.Message}");
                }
            }
        }
    }
}