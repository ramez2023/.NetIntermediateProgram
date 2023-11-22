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


--------------------------------------------------------------------------------------------------------------------------------------------------------
Common approaches to handle large messages include:
1- Message Chunking: Split large messages into smaller chunks and send them as multiple messages. 
   The receiving end needs to reassemble the chunks to reconstruct the original message.

2- Reference to External Storage: As mentioned in the previous response, send a reference (like a file path or URL) to the actual data stored externally. 
   This is useful when the message queue is not the ideal place for storing large payloads.

3- Compression: Compress the message payload before sending it. This reduces the overall size of the message. 
   However, it's effective only if the payload is compressible.

4- Streaming: Rather than sending the entire payload in one message, use streaming to send the data in smaller chunks or as a continuous stream. 
   The receiving end can process the data as it arrives.
*/

using RabbitMQ.Client;
using System.Buffers;
using Task01.Common;

class Program
{
    static void Main()
    {
        // Define a policy for handling transient errors
        string folderPath = FilePathManager.GetLocalFolderPathForDataInput();
        string[] supportedFormats = { ".pdf", ".mp4", ".mov", ".txt", ".dll" };

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
                Console.WriteLine($"New document detected: {e.FullPath}");

                string fileExtension = Path.GetExtension(e.FullPath).ToLower();

                if (Array.Exists(supportedFormats, format => format.Equals(fileExtension)))
                {
                    // await SendFileToQueueAtOnceAsync(channel, e.FullPath, fileExtension);
                    await SendFileToQueueAsChunksAsync(channel, e.FullPath, fileExtension);
                }
            };

            Console.WriteLine("Press 'Q' to quit the application.");
            while (Console.ReadKey().KeyChar != 'Q') { }
        }
    }

    static async Task SendFileToQueueAtOnceAsync(IModel channel, string filePath, string fileExtension)
    {
        await Task.Delay(3000); // 3 second

        byte[] fileBytes;
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
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

    static async Task SendFileToQueueAsChunksAsync(IModel channel, string filePath, string fileExtension)
    {
        await Task.Delay(3000); // 3 second
        const int chunkSize = 1024; // Set your desired chunk size
        int chunkNumber = 0;

        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(chunkSize);

            try
            {
                int bytesRead;
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Headers = new Dictionary<string, object>()
                        {
                            // Add more metadata as needed
                            { "fileName", fileName },
                            { "fileReference", filePath },
                            { "fileExtension", fileExtension },
                            { "chunkNumber", chunkNumber }
                        };

                    // body: buffer[..bytesRead] >>   is concise and clear but creates a new array, potentially leading to additional memory allocations.
                    // body: buffer.AsMemory(0, bytesRead).ToArray() >>   create a memory slice without copying the data. 
                    channel.BasicPublish(exchange: "", routingKey: RabbitMqHelper.QueueName, basicProperties: properties, body: buffer.AsMemory(0, bytesRead).ToArray());

                    Console.WriteLine($"Chunk {chunkNumber} sent to the queue: {fileName}");

                    // The purpose of returning the buffer inside the loop is to make the memory available for reuse as soon as possible.
                    // This can be beneficial when processing large files, as it helps manage memory efficiently.
                    ArrayPool<byte>.Shared.Return(buffer);
                    chunkNumber++;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}
