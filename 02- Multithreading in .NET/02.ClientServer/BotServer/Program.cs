
/*

Server: 
1- Accepts connections from the Client. On connecting receives the name of the Client.  
2- Receives messages from the Client and broadcasts them to all other clients connected to this Server.
3- Stores a history of N number (defined by you) of messages and sends this collection of messages to the Clients on their initial connection. 
4- Sends notification to the Clients and safely closes all connections on application close. 

 */

using BotServer;

class Server
{

    static async Task Main(string[] args)
    {
        //Task01.Run();
        await Task02.RunAsync();
    }
}
