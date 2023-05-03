using System;
using RabbitMQ.Client;
using System.Text;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Escreva sua mensagem: ");        
            var message = Console.ReadLine();

            while (message != "exit"){
                // message = Console.ReadLine();
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("AV AX4B", false, false, false, null);


                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish("", "AV AX4B", null, body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }
                }
                Console.WriteLine(" Digite 'exit' para sair.");
                message = Console.ReadLine();
            }

            // var child_ids = 2;
            // Console.WriteLine("Responsible");
            // Console.WriteLine(child_ids == 0 ? "RESPONSIBLE VAZIO" : "RESPONSIBLE PREENCHIDO");
            // Console.WriteLine("check box" );
            // if(child_ids != 1) {
            //     Console.WriteLine(child_ids == 0 ? "LICENCE VAZIO" : (child_ids == 2 ? "LICENCE PREENCHIDO" : ""));
            // }
        }
    }
}
