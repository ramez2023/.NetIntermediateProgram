using Common;
using System.Net.Sockets;

namespace BotClient
{
    public static class Task01
    {
        public static void Run()
        {
            TcpClient client = new TcpClient("127.0.0.1", 12345);
            NetworkStream networkStream = client.GetStream();

            string botName = Helper.ReturnRandomString();
            Console.WriteLine("your bot name: " + botName);
            networkStream.WriteString(botName);

            Thread receiveThread = new Thread(() => ReceiveMessages(networkStream));
            receiveThread.Start();

            while (true)
            {
                // Simulate a random delay before sending a message
                int delay = new Random().Next(1000, 5000); // Delay between 1 to 5 seconds
                Thread.Sleep(delay);

                // Generate a random message
                string message = GenerateRandomMessage();
                networkStream.WriteString(message);
            }

        }

        static void ReceiveMessages(NetworkStream networkStream)
        {
            while (true)
            {
                string message = networkStream.ReadString();
                Console.WriteLine(message);

                // You can also store the messages in a text file here.
                // Example: File.AppendAllText("messages.txt", message + Environment.NewLine);
            }
        }

        static string GenerateRandomMessage()
        {
            // Replace with your logic to generate random messages
            string[] predefinedMessages =
                {
                    "Hello, how are you?",
                    "What's up?",
                    "I'm a bot!",
                    "Nice to meet you.",
                    "Chatting is fun!" ,
                };

            int randomIndex = new Random().Next(predefinedMessages.Length);
            return predefinedMessages[randomIndex];
        }

    }
}
