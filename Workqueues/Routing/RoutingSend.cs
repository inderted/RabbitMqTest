using MqConnection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqModule.Routing
{
    public class RoutingSend
    {
        public void Send()
        {
            var conn = new GetConnection();
            conn.MqHandle((connection, channel) =>
            {
                //声明一个名为logs的交换机
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct", durable: true, autoDelete: false, null);

                //IBasicProperties.SetPersistent设置为true，用来将我们的消息标示成持久化的。
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                foreach (var item in Enum.GetValues(typeof(LogType)))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        var body = Encoding.UTF8.GetBytes(i.ToString());

                        //消息写入到hello队列中

                        ///param1:交换机名称
                        ///param2:队列名称
                        ///param3:传递消息额外设置
                        ///param4:消息具体内容
                        channel.BasicPublish(exchange: "direct_logs",
                                             routingKey: item.ToString(),
                                             basicProperties: properties,
                                             body: body);
                        Console.WriteLine(" [x] Sent {0}, Type {1}", i, item);
                    }
                }
            });

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public enum LogType
        {
            Info,
            Error,
            Warning
        }
    }
}
