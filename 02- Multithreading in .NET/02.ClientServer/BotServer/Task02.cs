using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BotServer
{
    public static class Task02
    {
        private static List<TcpClient> clients = new List<TcpClient>();
        private static ConcurrentQueue<string> messageHistory = new ConcurrentQueue<string>();
        private static int messageHistorySize = 10;

        public static async Task RunAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();

            Console.WriteLine("Server started. Listening for connections...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                _ = Task.Run(async () =>
                {
                    clients.Add(client);
                    string clientName = await ReceiveStringAsync(client.GetStream());


                    await SendHistoryAsync(client);

                    Console.WriteLine();

                    await BroadcastAsync(clientName + " has joined the chat.");

                    try
                    {
                        while (true)
                        {
                            string message = await ReceiveStringAsync(client.GetStream());

                            if (message == null)
                            {
                                clients.Remove(client);
                                await BroadcastAsync($"{clientName} has left the chat.");
                                client.Close();
                                break;
                            }

                            string fullMessage = $"{clientName}: {message}";
                            messageHistory.Enqueue(fullMessage + "\n");

                            if (messageHistory.Count > messageHistorySize)
                            {
                                messageHistory.TryDequeue(out _);
                            }

                            await BroadcastAsync(fullMessage);
                        }
                    }
                    catch
                    {
                        clients.Remove(client);
                        await BroadcastAsync($"{clientName} has left the chat.");
                        client.Close();
                    }
                });
            }
        }

        static async Task<string> ReceiveStringAsync(NetworkStream networkStream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

            if (bytesRead == 0)
            {
                return null;
            }

            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        static async Task SendStringAsync(NetworkStream networkStream, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            await networkStream.WriteAsync(data, 0, data.Length);
        }

        static async Task BroadcastAsync(string message)
        {
            Console.WriteLine(message);

            foreach (var client in clients)
            {
                await SendStringAsync(client.GetStream(), message);
            }
        }

        static async Task SendHistoryAsync(TcpClient client)
        {
            foreach (var message in messageHistory)
            {
                await SendStringAsync(client.GetStream(), message);
            }
        }
    }
}
