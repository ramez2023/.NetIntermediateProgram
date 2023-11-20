using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task01.Common
{
    public class RabbitMqHelper
    {
        public static string HostName = "localhost";
        public static int Port = 5672;
        public static string UserName = "guest";
        public static string Password = "guest";


        public static string QueueName = "file_data_queue";

    }
}
