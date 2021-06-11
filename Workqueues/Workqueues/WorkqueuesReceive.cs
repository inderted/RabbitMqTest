using MqConnection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MqModule.Workqueues
{
    public class WorkqueuesReceive
    {
        public void Receive()
        {
            var conn = new GetConnection();
            conn.MqHandle((connection, channel) =>
            {
                //声明一个名为hello的队列
                QueueDeclare.QueueDeclareByName("task_query", connection, channel);

                #region BasicQos
                //你可能注意到了，调度依照我们希望的方式运行。例如在有两个工作者的情况下，当所有的奇数任务都很繁重而所有的偶数任务都很轻松的时候，其中一个工作者会一直处于忙碌之中而另一个几乎无事可做。RabbitMQ并不会对此有任何察觉，仍旧会平均分配消息。

                //这种情况发生的原因是由于当有消息进入队列时，RabbitMQ只负责将消息调度的工作，而不会检查某个消费者有多少未经确认的消息。它只是盲目的将第n个消息发送给第n个消费者而已。

                //img

                //要改变这种行为的话，我们可以在BasicQos方法中设置prefetchCount = 1。这样会告诉RabbitMQ一次不要给同一个worker提供多于一条的信息。话句话说，在一个工作者还没有处理完消息，并且返回确认标志之前，不要再给它调度新的消息。取而代之，它会将消息调度给下一个不再繁忙的工作者
                #endregion
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                Console.WriteLine(" [*] Waiting for messages.");

                //接下来我们通知服务器可以将消息从队列里发送过来了。由于服务器会异步地将消息推送给我们，所以我们这里提供一个回调方法。这就是EventingBasicConsumer.Receivedevent所做的工作。
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (conn, ea) =>
                {
                    var body = ea.Body.Span;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                    Thread.Sleep(1000);
                    Console.WriteLine(" [x] Done");

                    //忘记使用BasicAck是一个常见的错误。虽然是个简单的错误，但是后果严重。消息会在客户端退出后重新投送（就像是随机进行的重新投送），但是由于RabbitMQ无法释放任何未经确认的消息，内存占用会越来越严重。
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                ///param1:消费哪个队列的消息 队列名称
                ///param2:开始消息的自动确认机制
                ///param3;消费时的回调接口
                channel.BasicConsume(queue: "task_query",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            });
        }
    }
}
