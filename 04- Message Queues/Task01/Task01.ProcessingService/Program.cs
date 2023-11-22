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

                await StoreChunksLocallyAsync(@event.Body.ToArray(), @event.BasicProperties.Headers);
                // await StoreMessageLocallyAsync(@event.Body.ToArray(), @event.BasicProperties.Headers);
            };

            // Start consuming messages
            channel.BasicConsume(queue: RabbitMqHelper.QueueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }

    static async Task StoreMessageLocallyAsync(byte[] fileBytes, IDictionary<string, object> headers)
    {
        string localFolder = FilePathManager.GetLocalFolderPathForDataOutput();
        var fileName = Encoding.UTF8.GetString((byte[])headers["fileName"]);
        var fileExtension = Encoding.UTF8.GetString((byte[])headers["fileExtension"]);
        string filePath = Path.Combine(localFolder, $"{fileName}{fileExtension}");

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await fileStream.WriteAsync(fileBytes, 0, fileBytes.Length);
        }

        Console.WriteLine($"Message stored locally: {filePath}");
    }

    static async Task StoreChunksLocallyAsync(byte[] chunk, IDictionary<string, object> headers)
    {
        string localFolder = FilePathManager.GetLocalFolderPathForDataOutput();
        var fileName = Encoding.UTF8.GetString((byte[])headers["fileName"]);
        var fileExtension = Encoding.UTF8.GetString((byte[])headers["fileExtension"]);
        string filePath = Path.Combine(localFolder, $"{fileName}{fileExtension}");
        
        var chunkNumber = headers["chunkNumber"];

        using (MemoryStream stream = new MemoryStream(chunk))
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        Console.WriteLine($"Chunk {chunkNumber} stored locally: {fileName}");
    }
}
