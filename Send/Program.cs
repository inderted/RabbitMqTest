using MqModule.Hello_World;
using MqModule.PublishSubscribe;
using MqModule.Routing;
using MqModule.Topics;
using MqModule.Workqueues;
using System;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Helloworld

            //var singleReceive = new SingleSend();
            //singleReceive.Send();

            #endregion

            #region workquery

            //var workQuery = new WorkqueuesSend();
            //workQuery.Send();

            #endregion

            #region PublishSubscribe

            //var publishSubscribe = new PublishSubscribeSend();
            //publishSubscribe.Send();

            #endregion

            #region Routing

            //var routing = new RoutingSend();
            //routing.Send();

            #endregion

            #region Topic

            var topic = new TopicsSend();
            topic.Send();

            #endregion
        }
    }
}
