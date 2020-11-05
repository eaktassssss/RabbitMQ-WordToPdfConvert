using System;
using WordToPdfConvert.Consumer.Service;

namespace WordToPdfConvert.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var consumer = new ConsumerService();
           consumer.Consumer();
            Console.ReadLine();
        }
    }
}
