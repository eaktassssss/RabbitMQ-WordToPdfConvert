using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordToPdfConvert.Consumer.Model;

namespace WordToPdfConvert.Consumer.Service
{
    public class ConsumerService
    {
        public void Consumer()
        {
            try
            {
                bool result = false;
                var factory = new ConnectionFactory();
                factory.HostName = "localhost";
                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {

                        channel.ExchangeDeclare(exchange: "file", type: ExchangeType.Direct, durable: true, autoDelete: false);
                        channel.QueueBind(queue: "wordtopdf", exchange: "file", routingKey: "wordtopdfconvert", null);
                        channel.BasicQos(0, 1, false);
                        var consumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: "wordtopdf", false, consumer: consumer);
                        consumer.Received += (obj, data) =>
                        {
                            Document document = new Document();
                            string message = Encoding.UTF8.GetString(data.Body.ToArray());
                            Message model = JsonConvert.DeserializeObject<Message>(message);

                            document.LoadFromStream(new MemoryStream(model.File), FileFormat.Docx2013);


                            using (MemoryStream memory = new MemoryStream())
                            {
                                document.SaveToStream(memory, FileFormat.PDF);
                                result = EmailService.Send(model.Email, memory, model.FileName);
                                if (result)
                                {
                                    channel.BasicAck(deliveryTag: data.DeliveryTag, false);
                                }
                            }
                        };


                    }
                }

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        
    }
}
