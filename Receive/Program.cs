using MqModule.HelloWorld;
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

            var workQuery = new WorkqueuesReceive();
            workQuery.Receive();

            #endregion
        }
    }
}
