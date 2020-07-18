using System;
using System.Collections.Generic;
using System.Text;

namespace Receive
{
    public class RabbitMQEventBusSettings
    {
        public string Application { get; set; } = "PrO";
        public string Service { get; set; }
        public int ConnectionRetryCount { get; set; } = 5;
        public RetryMessageProcessingSettings RetryMessageProcessing { get; set; }
        public RabbitMQConnectionSettings ConnectionSettings { get; set; }

    }
}
