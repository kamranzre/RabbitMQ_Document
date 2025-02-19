using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Receiver_Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = RabbitMQConfig.Instance.CreateConnection();
            var channel = RabbitMQConfig.Instance.CreateModel(connection);
            channel.QueueDeclare("MyQueue1", true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Random random = new Random();
                int sleep = random.Next(0, 3) * 1000;
                Console.WriteLine($"Sleep:{sleep} DeliveryTag{eventArgs.DeliveryTag}");
                Thread.Sleep(sleep);
                Console.WriteLine("Received Message " + message);

                // دستی است AutoAct این روش برای 
                channel.BasicAck(eventArgs.DeliveryTag,true);
            };

            // consumer در قسمت بالا ما یک   
            //ایجاد کردیم و بعدش  باید به صفی که باید اطلاعات را دریافت کند متصل کنیم
            //BasicConsume که با 
            //اینکار را انجام میدهیم

            // AutoAct
            //  پیغام را دریافت میکند Consumer میباشد که زمانی که  AutoAct دومین ورودی که دریافت میکند BasicConsume در 
            //یک پیغام به صف ارسال میکند و میگوید که پیغام دریافت شده
            //و صف پیغام را از داخل خود حذف میکند

            //AutoAct معایب 
            //صف خالی شده دیتا سمت دریافت کننده ارسال شده
            //اما دریافت کننده در هنگام پردازش کلیه اطلاعات با یک مشکل ناگهانی
            //مانند قطع برق یا کرش کردن یا مشکلات فنی 
            //پروژه ریست میشود
            //و بعد از ریست شدن اطلاعات که پردازش نشده از بین رفته

            //******راهکار******
            // دستی AutoAct استفاده از  
            //به این صورت که زمانی که پردازش به صورت کامل انجام شد آنگاه دستور حذفش را به صف ارسال میکنیم
            channel.BasicConsume("MyQueue1", false, consumer);
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
