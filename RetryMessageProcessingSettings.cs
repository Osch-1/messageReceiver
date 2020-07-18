using System;
using System.Collections.Generic;
using System.Text;

namespace Receive
{
    public class RetryMessageProcessingSettings
    {
        public int QueueWaitingTime { get; set; }
        public int TimeProcessInQueueSeconds { get; set; }
        public int AttemptCount { get; set; }
    }
}
