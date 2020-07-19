using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receive
{
    class Receiver
    {
        public static void Main()
        {            
            RabbitMQConnectionSettings connectionSettings = new RabbitMQConnectionSettings
            {
                HostName = "localhost",
                UserName = "test",
                Password = "test",
                VirtualHost = "/",
                UseSsl = false
            };
            RabbitMQEventBusSettings rabbitMQEventBusSettings = new RabbitMQEventBusSettings
            {
                Application = "PrO",
                Service = "Analytics",
                ConnectionSettings = connectionSettings,
                RetryMessageProcessing = new RetryMessageProcessingSettings 
                {
                    QueueWaitingTime = 5000,
                    TimeProcessInQueueSeconds = 1800,
                    AttemptCount = 10
                }
            };

            ConnectionFactory factory = RabbitMQPersistentConnection.CreateConnectionFactory(connectionSettings);
            RabbitMQPersistentConnection rabbitMQPersistentConnection = new RabbitMQPersistentConnection(factory, rabbitMQEventBusSettings);

            try
            {                
                if (rabbitMQPersistentConnection.TryConnect())
                {

                    using(var model = rabbitMQPersistentConnection.CreateModel())
                    {
                        string exchangeName = "PrO_events";
                        model.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true);

                        string queueName = "EventCS_Listener";
                        model.QueueDeclare(queue: queueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                        model.QueueBind(queue: queueName,
                                          exchange: exchangeName,
                                          routingKey: "#");

                        Console.WriteLine(" [*] Waiting for messages.");

                        var consumer = new EventingBasicConsumer(model);

                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(" [x] {0}", message);
                        };

                        model.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                        
                    }
                }                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            rabbitMQPersistentConnection.Dispose();
        }
    }
}