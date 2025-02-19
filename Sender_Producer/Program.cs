using RabbitMQ.Client;
using System.Text;
using System;
using System.Threading;

namespace Sender_Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = RabbitMQConfig.Instance.CreateConnection();
            var channel = RabbitMQConfig.Instance.CreateModel(connection);

            //Create queue

            //durable //زمانی که به هر دلیلی rabbit متوقف شود یا ریست شود صف ها پابرجا میمانند
            //اما دیتا ها ازبین میرود

            //Exclusive
            //Consumer تا زمانی که دریافت کننده یا همان
            //متصل بود و کانکشنش باز بود صف موجود است 
            //زمانی که کانکشن بسته شود صف نیز حذف میشود

            //autoDelete
            //زمانی که تمامی پیغام ها تحویل داده شود و پروژه بسته شود صف حذف میشود 
            channel.QueueDeclare("MyQueue1", true, false, false, null);


            //برای تغییر اصول ارسال پیام از 
            //Round-robin
            //به
            //Fair Dispatch
            //اینگونه عمل میکنیم
            channel.BasicQos(0, 1, false);

            string Message = $"THis a Test Message From My Sender Ad:{DateTime.Now.Ticks}";

            //Put Message To Queue
            //First of All We have to Convert My Message to Byte
            var body = Encoding.UTF8.GetBytes(Message);
            for (int i = 0; i < 100; i++)
            {
                #region Persistent
                //نگه داری اطلاعات درون صف در هارد به جای رم
                //تا اگر ربیت ریست شد اطلاعات باقی بماند
                var proprties = channel.CreateBasicProperties();
                proprties.Persistent = true;
                #endregion

                //Send body To Queue
                channel.BasicPublish("", "MyQueue1", proprties, body);
            }

            channel.Close();
            connection.Close();
            Console.ReadLine();

            Console.WriteLine("Hello World!");
        }
    }
}
