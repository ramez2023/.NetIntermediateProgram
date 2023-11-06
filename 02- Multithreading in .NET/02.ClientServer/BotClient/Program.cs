/*

Client and Server interacts with each other using Named Pipes (System.IO.Pipes) or Sockets (System.Net.Sockets) – you decide. 
For simplicity, connection parameters could be hardcoded. 

Client is a bot which performs the following operations in a loop: 
1- Connects to the Server with a new name 
2- Sends several messages to the Server with a short delay between messages. 
   (Messages are retrieved from the list of already predefined messages, 
    the number of messages and delay between messages is random) 
3- Receives all messages from the Server and displays them on the screen or stores in a text file. 
4- Disconnects from the Server. 

Repeat the loop until the User stops the Client or exception occurred. 
 * */

using BotClient;

class Client
{
    static async Task Main(string[] args)
    {
        //Task01.Run();
        await Task02.RunAsync();
    }

}
