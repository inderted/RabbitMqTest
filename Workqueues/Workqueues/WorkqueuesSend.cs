using MqConnection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqModule.Workqueues
{
    public class WorkqueuesSend
    {
        public void Send()
        {
            var conn = new GetConnection();
            conn.MqHandle((connection, channel) =>
            {
                //声明一个名为hello的队列
                QueueDeclare.QueueDeclareByName("task_query", connection, channel);

                //IBasicProperties.SetPersistent设置为true，用来将我们的消息标示成持久化的。
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                #region 关于消息持久化的注释

                //消息持久化的注释
                //将消息标示为持久化并不能完全保证消息不会丢失。尽管它会告诉RabbitMQ将消息存储到硬盘上，但是在RabbitMQ接收到消息并将其进行存储两个行为之间仍旧会有一个窗口期。同样的，RabbitMQ也不会对每一条消息执行fsync(2)，所以消息获取只是存到了缓存之中，而不是硬盘上。虽然持久化的保证不强，但是应对我们简单的任务队列已经足够了

                #endregion

                for (int i = 0; i < 1000; i++)
                {
                    var body = Encoding.UTF8.GetBytes(i.ToString());

                    //消息写入到hello队列中

                    ///param1:交换机名称
                    ///param2:队列名称
                    ///param3:传递消息额外设置
                    ///param4:消息具体内容
                    channel.BasicPublish(exchange: "",
                                         routingKey: "task_query",
                                         basicProperties: properties,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", i);
                }
            });

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
