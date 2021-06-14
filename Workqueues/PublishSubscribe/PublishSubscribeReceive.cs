using MqConnection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MqModule.PublishSubscribe
{
    public class PublishSubscribeReceive
    {
        public void Receive()
        {
            var conn = new GetConnection();
            conn.MqHandle((connection, channel) =>
            {
                //声明一个名为logs的交换机
                channel.ExchangeDeclare(exchange: "logs", type: "fanout", durable: true, autoDelete: false, null);
                //创建一个随机队列
                var queryName = channel.QueueDeclare(queue: string.Empty, durable: true, exclusive: false, autoDelete: true).QueueName;
                //队列绑定到交换机logs
                channel.QueueBind(queue: queryName,
                    exchange: "logs",
                    routingKey: "");
                Console.WriteLine(" [*] Waiting for messages.");

                //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                //接下来我们通知服务器可以将消息从队列里发送过来了。由于服务器会异步地将消息推送给我们，所以我们这里提供一个回调方法。这就是EventingBasicConsumer.Receivedevent所做的工作。
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (conn, ea) =>
                {
                    var body = ea.Body.Span;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                    Thread.Sleep(1000);
                    Console.WriteLine(" [x] Done");
                };

                ///param1:消费哪个队列的消息 队列名称
                ///param2:开始消息的自动确认机制
                ///param3;消费时的回调接口
                channel.BasicConsume(queue: queryName,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            });
        }
    }
}
