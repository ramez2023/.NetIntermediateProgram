using System.Net.Sockets;
using System.Text;

namespace Common
{
    public static class NetworkStreamExtensions
    {
        public static string ReadString(this NetworkStream stream)
        {
            byte[] data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            return Encoding.ASCII.GetString(data, 0, bytesRead);
        }

        public static void WriteString(this NetworkStream stream, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}