using MqConnection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MqModule.HelloWorld
{
    public class SingleReceive
    {
        public void Receive()
        {
            var conn = new GetConnection();
            conn.MqHandle((connection, channel) =>
            {
                //声明一个名为hello的队列
                QueueDeclare.QueueDeclareByName("hello", connection, channel);

                //接下来我们通知服务器可以将消息从队列里发送过来了。由于服务器会异步地将消息推送给我们，所以我们这里提供一个回调方法。这就是EventingBasicConsumer.Receivedevent所做的工作。
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (conn, ea) =>
                {
                    ///Thread.Sleep(2000);
                    var body = ea.Body.Span;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };

                ///param1:消费哪个队列的消息 队列名称
                ///param2:开始消息的自动确认机制
                ///param3;消费时的回调接口
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            });
        }
    }
}
