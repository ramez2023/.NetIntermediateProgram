using Common;
using System.Net;
using System.Net.Sockets;

namespace BotServer
{
    public static class Task01
    {
        private static List<Socket> clients = new List<Socket>();
        private static List<string> messageHistory = new List<string>();
        private static object lockObj = new object();
        private static int messageHistorySize = 10;

        public static void Run()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();

            Console.WriteLine("Server started. Listening for connections...");

            while (true)
            {
                Socket client = listener.AcceptSocket();
                clients.Add(client);

                string clientName = new NetworkStream(client).ReadString();

                SendHistory(client);

                Console.WriteLine();

                Broadcast(clientName + " has joined the chat.");


                Thread clientThread = new Thread(() => HandleClient(client, clientName));
                clientThread.Start();
            }

        }

        static void HandleClient(Socket client, string clientName)
        {
            NetworkStream networkStream = new NetworkStream(client);

            try
            {
                while (true)
                {
                    string message = networkStream.ReadString();

                    if (message == null)
                    {
                        lock (lockObj)
                        {
                            clients.Remove(client);
                        }
                        client.Close();
                        Broadcast(clientName + " has left the chat.");
                        break;
                    }

                    string fullMessage = $"{clientName}: {message}";
                    messageHistory.Add(fullMessage + "\n");

                    if (messageHistory.Count > messageHistorySize)
                    {
                        messageHistory.RemoveAt(0);
                    }

                    Broadcast(fullMessage);
                }
            }
            catch (IOException)
            {
                // Handle the IOException caused by a client disconnect.
                lock (lockObj)
                {
                    clients.Remove(client);
                }
                client.Close();
                Broadcast(clientName + " has left the chat.");
            }
        }

        static void SendHistory(Socket client)
        {
            lock (lockObj)
            {
                foreach (var message in messageHistory)
                {
                    new NetworkStream(client).WriteString(message);
                }
            }
        }

        static void Broadcast(string message)
        {
            Console.WriteLine(message);

            lock (lockObj)
            {
                foreach (var c in clients)
                {
                    new NetworkStream(c).WriteString(message);
                }
            }
        }

    }
}
