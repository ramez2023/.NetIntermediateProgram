using Common;
using System.Net.Sockets;
using System.Text;

namespace BotClient
{
    public static class Task02
    {
        public static async Task RunAsync()
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 12345);

            NetworkStream networkStream = client.GetStream();

            string botName = Helper.ReturnRandomString();
            Console.WriteLine("your bot name: " + botName);
            await SendStringAsync(networkStream, botName);

            Task receiveTask = ReceiveMessagesAsync(networkStream);

            while (true)
            {
                // Simulate a random delay before sending a message
                int delay = new Random().Next(1000, 5000); // Delay between 1 to 5 seconds
                await Task.Delay(delay);

                // Generate a random message
                string message = GenerateRandomMessage();
                await SendStringAsync(networkStream, message);
            }
        }

        static async Task ReceiveMessagesAsync(NetworkStream networkStream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (true)
            {
                bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    Console.WriteLine("Server has closed the connection.");
                    break;
                }

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine(message);
            }
        }

        static async Task SendStringAsync(NetworkStream networkStream, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            await networkStream.WriteAsync(data, 0, data.Length);
        }

        static string GenerateRandomMessage()
        {
            // Replace with your logic to generate random messages
            string[] predefinedMessages = {
            "Hello, how are you?",
            "What's up?",
            "I'm a bot!",
            "Nice to meet you.",
            "Chatting is fun!",
        };

            int randomIndex = new Random().Next(predefinedMessages.Length);
            return predefinedMessages[randomIndex];
        }
    }
}
