using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    TcpListener server;
    TcpClient client;
    NetworkStream stream;

    Thread receiveThread;

    public bool Connected => client != null && client.Connected;

    public Action<string> OnMessageReceived;

    const int PORT = 7777;

    public void StartServer()
    {
        try
        {
            server = new TcpListener(IPAddress.Any, PORT);
            server.Start();

            Debug.Log("Servidor iniciado.");

            server.BeginAcceptTcpClient(OnClientConnected, null);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    void OnClientConnected(IAsyncResult result)
    {
        client = server.EndAcceptTcpClient(result);
        stream = client.GetStream();

        Debug.Log("Cliente conectado!");

        receiveThread = new Thread(ReceiveLoop);
        receiveThread.Start();
    }

    public void ConnectToServer(string ip)
    {
        try
        {
            client = new TcpClient();

            client.Connect(ip, PORT);

            stream = client.GetStream();

            Debug.Log("Conectado ao servidor!");

            receiveThread = new Thread(ReceiveLoop);
            receiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    void ReceiveLoop()
    {
        byte[] buffer = new byte[1024];

        while (client != null && client.Connected)
        {
            int length = stream.Read(buffer, 0, buffer.Length);

            if (length > 0)
            {
                string msg = Encoding.UTF8.GetString(buffer, 0, length);

                OnMessageReceived?.Invoke(msg);
            }
        }
    }

    public void Send(string message)
    {
        if (!Connected)
            return;

        byte[] data = Encoding.UTF8.GetBytes(message);

        stream.Write(data, 0, data.Length);
    }

    private void OnApplicationQuit()
    {
        receiveThread?.Abort();

        stream?.Close();

        client?.Close();

        server?.Stop();
    }
}