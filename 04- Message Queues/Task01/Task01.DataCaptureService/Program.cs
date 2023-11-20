/*

****************************** Data capture service ******************************

Implement Data capture service which will 
- listen to a specific local folder and retrieve documents of some specific format (i.e., PDF) and 
- send to Main processing service through message queue. 

Try to experiment with your system like sending large files (300-500 Mb), if necessary, configure your system to use another format (i.e. .mp4). 
For learning purposes assume that there could be multiple Data capture services, but we can have only one Processing server. 

Notes:
One of the challenges in this task is that we have a limit for message size. Message queues have limits for a single message size.  
Please find one of the approaches to bypass this limitation by clicking the link

*/

using RabbitMQ.Client;
using Task01.Common;

class Program
{
    static void Main()
    {
        string folderPath = FilePathManager.GetLocalFolderPathForDataInput();
        string[] supportedFormats = { ".pdf", ".mp4", ".txt" };

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
            channel.QueueDeclare(queue: RabbitMqHelper.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var watcher = new FileSystemWatcher(folderPath)
            {
                EnableRaisingEvents = true,
            };

            watcher.Created += async (sender, e) =>
            {
                string fileExtension = Path.GetExtension(e.FullPath).ToLower();

                if (Array.Exists(supportedFormats, format => format.Equals(fileExtension)))
                {
                    Console.WriteLine($"New document detected: {e.FullPath}");
                    await SendToQueueAsync(channel, e.FullPath, fileExtension);
                }
            };

            Console.WriteLine("Press 'Q' to quit the application.");
            while (Console.ReadKey().KeyChar != 'Q') { }
        }
    }

    static async Task SendToQueueAsync2(IModel channel, string filePath, string fileExtension)
    {
        byte[] fileBytes;
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            fileBytes = new byte[fileStream.Length];
            await fileStream.ReadAsync(fileBytes, 0, fileBytes.Length);
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            var properties = channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object>()
            {
                { "fileName", fileName },
                { "fileExtension", fileExtension }
            };

            channel.BasicPublish(exchange: "", routingKey: RabbitMqHelper.QueueName, basicProperties: properties, body: fileBytes);

            Console.WriteLine($"Document sent to the queue: {filePath}");
        }
    }
    
    static async Task SendToQueueAsync(IModel channel, string filePath, string fileExtension)
    {
        const int maxAttempts = 3;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            try
            {
                byte[] fileBytes;
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fileBytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(fileBytes, 0, fileBytes.Length);
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    var properties = channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<string, object>()
                    {
                        { "fileName", fileName },
                        { "fileExtension", fileExtension }
                    };

                    channel.BasicPublish(exchange: "", routingKey: RabbitMqHelper.QueueName, basicProperties: properties, body: fileBytes);

                    Console.WriteLine($"Document sent to the queue: {filePath}");
                }
                break;
            }
            catch (IOException)
            {
                await Task.Delay(1000); // 1 second
                attempts++;
            }
        }

        if (attempts == maxAttempts)
        {
            throw new IOException();
        }
    }
}
