/*

****************************** processing service ******************************

Implement the main processing service that should do the following: 
- Create new queue on startup for receiving results from Data capture services. 
- Listen to the queue and store  all incoming messages in a local folder. 


*/

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Task01.Common;

class Program
{
    static void Main()
    {
        Console.WriteLine("Main Processing Service is running...");

        var factory = new ConnectionFactory
        {
            HostName = RabbitMqHelper.HostName,
            Port = RabbitMqHelper.Port,
            UserName = RabbitMqHelper.UserName,
            Password = RabbitMqHelper.Password
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: RabbitMqHelper.QueueName, durable: true, exclusive: false, autoDelete: false);

            // Set up consumer to listen to the queue
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, @event) =>
            {
                var fileNameHeader = Encoding.UTF8.GetString((byte[])@event.BasicProperties.Headers["fileName"]);
                var fileExtensionHeader = Encoding.UTF8.GetString((byte[])@event.BasicProperties.Headers["fileExtension"]);

                await StoreMessageLocallyAsync(@event.Body.ToArray(), fileNameHeader, fileExtensionHeader);
            };

            // Start consuming messages
            channel.BasicConsume(queue: RabbitMqHelper.QueueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }

    static async Task StoreMessageLocallyAsync(byte[] fileBytes, string fileName, string fileExtension)
    {
        string localFolder = FilePathManager.GetLocalFolderPathForDataOutput();
        string filePath = Path.Combine(localFolder, $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}");

        await File.WriteAllBytesAsync(filePath, fileBytes);

        Console.WriteLine($"Message stored locally: {filePath}");
    }
}
