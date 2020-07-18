using System;
using System.Collections.Generic;
using System.Text;

namespace Receive
{
    public class RabbitMQConnectionSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public bool UseSsl { get; set; }

    }
}
