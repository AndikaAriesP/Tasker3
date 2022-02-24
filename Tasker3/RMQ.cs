using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasker3
{
    class RMQ
    {
        public RabbitMQ.Client.ConnectionFactory connectionFactory;
        public RabbitMQ.Client.IConnection connection;
        public RabbitMQ.Client.IModel channel;
        public void InitRMQConnection(string host = "localhost", int port = 5672, string user = "guest",
        string pass = "guest")
        {
            connectionFactory = new RabbitMQ.Client.ConnectionFactory();
            connectionFactory.HostName = host;
            //connectionFactory.Port = port;
            connectionFactory.UserName = user;
            connectionFactory.Password = pass;
        }
        public void CreateRMQConnection()
        {
            connection = connectionFactory.CreateConnection();
            Console.WriteLine("Koneksi " + (connection.IsOpen ? "Berhasil!" : "Gagal!"));
        }
        public void CreateRMQChannel(string queue_name, string routingKey = "", string exchange_name =
        "")
        {
            if (connection.IsOpen)
            {
                channel = connection.CreateModel();
                Console.WriteLine("Channel " + (channel.IsOpen ? "Berhasil!" : "Gagal!"));
            }
            if (channel.IsOpen)
            {
                channel.QueueDeclare(queue: queue_name,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
                Console.WriteLine("Queue telah dideklarasikan..");
            }
        }
        public void SendMessage(string tujuan, string msg = "send")
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(msg);// konversi pesan dalam bentuk string menjadi byte
            channel.BasicPublish(exchange: "",
            routingKey: tujuan,
            basicProperties: null,
            body: responseBytes);
            Console.WriteLine("Pesan: '" + msg + "' telah dikirim.");
        }
        public void Disconnect()
        {
            channel.Close();
            channel = null;
            Console.WriteLine("Channel ditutup!");
            if (connection.IsOpen)
            {
                connection.Close();
            }
            Console.WriteLine("Koneksi diputus!");
            connection.Dispose();
            connection = null;
        }
    }

}
