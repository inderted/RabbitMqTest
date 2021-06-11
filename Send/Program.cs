using MqModule.Hello_World;
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

            var workQuery = new WorkqueuesSend();
            workQuery.Send();

            #endregion
        }
    }
}
