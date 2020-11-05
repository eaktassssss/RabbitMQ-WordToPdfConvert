using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using WordToPdfConvert.Web.Publisher.Models;

namespace WordToPdfConvert.Web.Publisher.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult WordToPdfConvert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WordToPdfConvert(Content content)
        {
            if (!ModelState.IsValid)
                return View(content);
            else
            {
                try
                {
                    var factory = new ConnectionFactory();
                    factory.HostName = "localhost";
                    using (var connection = factory.CreateConnection())
                    {
                        using (var channel = connection.CreateModel())
                        {
                            channel.ExchangeDeclare(exchange: "file", type: ExchangeType.Direct, durable: true, autoDelete: false, null);
                            channel.QueueDeclare(queue: "wordtopdf", durable: true, exclusive: false, autoDelete: false, null);
                            channel.QueueBind(queue: "wordtopdf", exchange: "file", routingKey: "wordtopdfconvert", null);
                            IBasicProperties properties = channel.CreateBasicProperties();
                            properties.Persistent = true;
                            var message = new Message();
                            using (var stream = new MemoryStream())
                            {
                                content.File.CopyTo(stream);
                                message.File = stream.ToArray();
                            }
                            message.Email = content.Email;
                            message.FileName = Path.GetFileNameWithoutExtension(content.File.FileName);
                            string data = JsonConvert.SerializeObject(message);
                            byte[] body = Encoding.UTF8.GetBytes(data);
                            channel.BasicPublish(exchange: "file", routingKey: "wordtopdfconvert", properties, body: body);
                        }
                    }
                    return RedirectToAction("WordToPdfConvert", "File");
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
            }

        }
    }
}