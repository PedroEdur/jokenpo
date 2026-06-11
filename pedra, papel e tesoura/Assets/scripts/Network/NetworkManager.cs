using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Network
{
    public class NetworkManager : MonoBehaviour
    {
        public event Action<string> OnMessageReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;

        [SerializeField] private string host = "127.0.0.1";
        [SerializeField] private int port = 7777;

        private TcpListener server;
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread receiveThread;
        private readonly ConcurrentQueue<string> receivedMessages = new ConcurrentQueue<string>();
        private bool isRunning;

        private void Update()
        {
            while (receivedMessages.TryDequeue(out string message))
            {
                OnMessageReceived?.Invoke(message);
            }
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }

        public void StartServer()
        {
            if (isRunning)
            {
                return;
            }

            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            isRunning = true;

            Thread acceptThread = new Thread(AcceptClient);
            acceptThread.IsBackground = true;
            acceptThread.Start();

            Debug.Log($"Server started on port {port}");
        }

        public void ConnectToServer()
        {
            if (isRunning)
            {
                return;
            }

            try
            {
                client = new TcpClient(host, port);
                PrepareStreams();
                StartReceiveThread();
                OnConnected?.Invoke();
            }
            catch (SocketException exception)
            {
                Debug.LogError($"Connection failed: {exception.Message}");
            }
        }

        public void Send(string message)
        {
            if (writer == null)
            {
                Debug.LogWarning("Cannot send message because there is no active TCP connection.");
                return;
            }

            writer.WriteLine(message);
            writer.Flush();
        }

        public void Disconnect()
        {
            isRunning = false;

            reader?.Close();
            writer?.Close();
            client?.Close();
            server?.Stop();

            reader = null;
            writer = null;
            client = null;
            server = null;

            OnDisconnected?.Invoke();
        }

        private void AcceptClient()
        {
            try
            {
                client = server.AcceptTcpClient();
                PrepareStreams();
                StartReceiveThread();
                OnConnected?.Invoke();
            }
            catch (SocketException exception)
            {
                if (isRunning)
                {
                    Debug.LogError($"Accept client failed: {exception.Message}");
                }
            }
        }

        private void PrepareStreams()
        {
            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            isRunning = true;
        }

        private void StartReceiveThread()
        {
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private void ReceiveLoop()
        {
            while (isRunning && client != null && client.Connected)
            {
                try
                {
                    string message = reader.ReadLine();

                    if (string.IsNullOrEmpty(message))
                    {
                        continue;
                    }

                    receivedMessages.Enqueue(message);
                }
                catch (IOException)
                {
                    Disconnect();
                }
                catch (ObjectDisposedException)
                {
                    Disconnect();
                }
            }
        }
    }
}