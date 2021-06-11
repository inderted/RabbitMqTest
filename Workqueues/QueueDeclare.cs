using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqModule
{
    public class QueueDeclare
    {
        /// <summary>
        /// 声明队列
        /// 在生产消息和消费消息端同时声明队列。这是因为，我们可能会先把消费者启动起来，而不是发布者。我们希望确保用于消费的队列是确实存在的。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connection"></param>
        /// <param name="channel"></param>
        public static void QueueDeclareByName(string name, IConnection connection, IModel channel)
        {
            ///param1:队列名称 如果队列不存在自动创建
            ///param2:用于定义队列是否需要持久化， true 持久化，false 不持久化
            ///param3:exclusive 是否独占队列 true 独占 ，false 不独占（独占表示只有当前链接独占可用，在使用时，其他链接不可用）
            ///param4:autoDelete 是否在消费完成后自动删除队列， true 自动删除， false 不自动删除
            ///param5:额外附加参数
            channel.QueueDeclare(queue: name,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
        }
    }
}
