using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateConnection(MqType.Send);
            CreateConnection(MqType.Received);

        }

        public static void CreateConnection(MqType type)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "***";//主机名，Rabbit会拿这个IP生成一个endpoint，这个很熟悉吧，就是socket绑定的那个终结点。
            factory.UserName = "***";//默认用户名,用户可以在服务端自定义创建，有相关命令行
            factory.Password = "***";//默认密码
            factory.Port = 5672;

            using (var connection = factory.CreateConnection())//连接服务器，即正在创建终结点。
            {
                if (type == MqType.Send)
                {
                    Send(connection);
                }
                else
                {
                    Received(connection);
                }
            }
        }

        /// <summary>
        /// 生产
        /// </summary>
        /// <param name="connection"></param>
        public static void Send(IConnection connection)
        {
            //创建一个通道，这个就是Rabbit自己定义的规则了，如果自己写消息队列，这个就可以开脑洞设计了
            //这里Rabbit的玩法就是一个通道channel下包含多个队列Queue
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("kibaQueue", false, false, false, null);//创建一个名称为kibaqueue的消息队列
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 1;
                string message = "I am Kiba518"; //传递的消息内容
                channel.BasicPublish("", "kibaQueue", properties, Encoding.UTF8.GetBytes(message)); //生产消息
                Console.WriteLine($"Send:{message}");
            }
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="connection"></param>
        public static void Received(IConnection connection)
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("kibaQueue", false, false, false, null);

                /* 这里定义了一个消费者，用于消费服务器接受的消息
                 * C#开发需要注意下这里，在一些非面向对象和面向对象比较差的语言中，是非常重视这种设计模式的。
                 * 比如RabbitMQ使用了生产者与消费者模式，然后很多相关的使用文章都在拿这个生产者和消费者来表述。
                 * 但是，在C#里，生产者与消费者对我们而言，根本算不上一种设计模式，他就是一种最基础的代码编写规则。
                 * 所以，大家不要复杂的名词吓到，其实，并没那么复杂。
                 * 这里，其实就是定义一个EventingBasicConsumer类型的对象，然后该对象有个Received事件，
                 * 该事件会在服务接收到数据时触发。
                 */
                var consumer = new EventingBasicConsumer(channel);//消费者
                channel.BasicConsume("kibaQueue", true, consumer);//消费消息
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(message);
                };
            }
        }

        public enum MqType
        {
            //生产
            Send,
            //消费
            Received
        }
    }
}
