using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace AssaultCubeCheatMenu.Networking
{
    public class Client
    {
        private TcpClient _tcpClient;
        private StreamReader _reader;
        private StreamWriter _writer;
        private Thread _receiveThread;

        public event Action<string> OnMessageReceived;

        public bool IsConnected => _tcpClient?.Connected ?? false;

        public void Connect(string ip, int port)
        {
            try
            {
                _tcpClient = new TcpClient(ip, port);
                NetworkStream stream = _tcpClient.GetStream();
                _reader = new StreamReader(stream, Encoding.UTF8);
                _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                // Start listening for messages
                _receiveThread = new Thread(ReceiveMessages) { IsBackground = true };
                _receiveThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to server: {ex.Message}");
            }
        }

        public void SendMessage(string message)
        {
            if (_writer != null && IsConnected)
            {
                _writer.WriteLine(message);
            }
        }

        public void Disconnect()
        {
            _receiveThread?.Abort();
            _reader?.Close();
            _writer?.Close();
            _tcpClient?.Close();
        }

        private void ReceiveMessages()
        {
            try
            {
                while (IsConnected)
                {
                    string message = _reader.ReadLine();
                    if (message != null)
                    {
                        OnMessageReceived?.Invoke(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving messages: {ex.Message}");
            }
        }
    }
}
