using MqConnection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqModule.Hello_World
{
    public class SingleSend
    {
        public void Send()
        {
            var conn = new GetConnection();
            conn.MqHandle((connection, channel) =>
            {
                //声明一个名为hello的队列
                QueueDeclare.QueueDeclareByName("hello", connection, channel);

                for (int i = 0; i < 1000; i++)
                {
                    var body = Encoding.UTF8.GetBytes(i.ToString());

                    //消息写入到hello队列中

                    ///param1:交换机名称
                    ///param2:队列名称
                    ///param3:传递消息额外设置
                    ///param4:消息具体内容
                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", i);
                }
            });

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
