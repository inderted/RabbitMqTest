using MqModule.HelloWorld;
using MqModule.PublishSubscribe;
using MqModule.Routing;
using MqModule.Topics;
using MqModule.Workqueues;
using System;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Helloworld

            //var singleReceive = new SingleReceive();
            //singleReceive.Receive();

            #endregion

            #region workquery

            //var workQuery = new WorkqueuesReceive();
            //workQuery.Receive();


            #region 总结

            //使用消息确认和BasicQos，你可以设置一个工作队列。持久化选项可以使得任务即使在RabbitMQ重启后也不会丢失
            //ack的作用是保证了一个耗时的消费不会因为消费端挂掉导致消息丢失的情况，在消息被成功消费后，会推送一个消息确认ack到rabbitmq中，确认消息已经处理完成，可以删除
            //队列消息持久化，保证了即使在mq挂掉的情况下，消息不丢失，但->
            //同样的，RabbitMQ也不会对每一条消息执行fsync(2)，所以消息获取只是存到了缓存之中，而不是硬盘上。
            //虽然持久化的保证不强，但是应对我们简单的任务队列已经足够了。如果你需要更强的保证，可以使用publisher confirms.

            //BasicQos告诉RabbitMQ一次不要给同一个worker提供多于一条的信息。话句话说，在一个工作者还没有处理完消息，并且返回确认标志之前，不要再给它调度新的消息。
            //取而代之，它会将消息调度给下一个不再繁忙的工作者

            #endregion

            #endregion

            #region PublishSubscribe

            //var publishSubscribe = new PublishSubscribeReceive();
            //publishSubscribe.Receive();

            #region 总结

            //发布订阅模式，更类似广播的形式，生产者发布消息到交换机exchange，所有绑定到这个交换机的队列都会收到生产者同样的消息，前提是，这个临时队列已经生产，并绑定到交换机

            #endregion

            #endregion

            #region Routing

            //var routing = new RoutingReceive();
            //routing.Receive(args);

            #region 总结

            //direct模式与fanout模式相比添加了一个绑定建：routingKey，用于获取广播的一个子集，是对fanout模式的扩展，通过routingKey绑定建，绑定到不同的临时队列，达到订阅子集的效果

            #endregion

            #endregion

            #region Topic

            #region Topic

            var topic = new TopicsReceive();
            topic.Receive(args);

            #region 总结

            //`*` (星号)能够替代一个单词。
            //`#` (井号) 能够替代零个或多个单词。

            //主题交换机非常强大，并且可以表现的跟其他交换机相似。

            //当一个队列使用"#"（井号）绑定键进行绑定。它会表现的像扇形交换机一样，不理会路由键，接收所有消息。

            //当绑定当中不包含任何一个 "*"(星号) 和 "#"(井号)特殊字符的时候，主题交换机会表现的跟直连交换机一毛一样。

            //Topic模式相当于是在direct模式基础上丰富了消息筛选的功能，可以选择性的消费消息

            #endregion

            #endregion

            #endregion
        }
    }
}
