using RabbitMQ.Client;
using System;

namespace MqConnection
{
    public class GetConnection
    {
        public void MqHandle(Action<IConnection, IModel> handleMethod)
        {
            var factory = GetConnectionFactory();
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    handleMethod(connection, channel);
                }
            }
        }

        /// <summary>
        /// 获取链接
        /// </summary>
        /// <returns></returns>
        public ConnectionFactory GetConnectionFactory()
        {
            return new ConnectionFactory()
            {
                HostName = "8.129.91.122",//主机名，Rabbit会拿这个IP生成一个endpoint，这个很熟悉吧，就是socket绑定的那个终结点。
                UserName = "inder",//默认用户名,用户可以在服务端自定义创建，有相关命令行
                Password = "y3909039",//默认密码
                Port = 5672
            };
        }
    }

    public enum MqType
    {
        //生产
        Send,
        //消费
        Received
    }
}
