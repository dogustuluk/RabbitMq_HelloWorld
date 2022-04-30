using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //subscriber >> consumer olarak da adlandırılabilir.
            //RabbitMQ ile haberleşebilmek için dependencies'e "RabbitMQ.Client" paketini yüklememiz gerekir.

            var factory = new ConnectionFactory(); //rabbitMQ'ya bağlanmak için bir "Connection Class" oluşturmamız lazım.
            factory.Uri = new Uri("amqps://nxdranwu:n_3nr-xZlXx0NoCWuFP05gTqZfp7_hwK@sparrow.rmq.cloudamqp.com/nxdranwu ");
            //uri'yi rabbit cloud'taki -CloudAMQP- instance'ımızın içerisinden buluyoruz. AMQP URL'i buraya yapıştırıyoruz.
            //gerçek hayat senaryolarında buraya uri'yi direkt olarak yapıştırmayız, bu bilgileri appSetting.cs'in içerisinde tutarız.
            //uri yazıldıktan sonra rabbitMQ ile bağlantı sağlamış oluyoruz.
            //------
            //şimdi bağlantı açmamız gerekmektedir. using ifadesi ile açıldı.
            using var connection = factory.CreateConnection(); //connection Main scope'u bitince düşmektedir.

            //bağlantı açıldı. Şimdi rabbitMQ'ya bu bağlantı aracılığıyla bir kanal ile bağlanmamız gerekiyor. 
            var channel = connection.CreateModel(); //kanal oluştu. Artık bu kanal üzerinden rabbitMQ ile haberleşilecektir. 
            // channel.QueueDeclare("hello-queue", true, false, false);

            var consumer =new EventingBasicConsumer(channel); //subscriber oluşturuldu.

            channel.BasicConsume("hello-queue", true, consumer); //hangi kuyruğu dinleyeceğini belirtiyoruz.
            //BasicConsume yapısı >>> (string queue, bool autoAck, consumer)
            //string queue >>> kuyruğun adı
            //bool autoAck >> eğer true olursa subscriber'a iletilen mesaj doğru da işlense yanlış da işlense rabbitMQ yollanan
                    //bu mesajı siliyor.
                    //eğer false olursa rabbitMQ'ya bu mesajı subscriber'a yolladıktan hemen sonra silme, mesajın doğru işlenip
                    //işlenmediğinin kontrolünü yaptıktan sonra sana yolladığım geri bildirim doğrultusunda kuyruktan sil demektir.


            //received eventinin aksiyon alması rabbitMQ subscriber'a mesaj yolladığında olacaktır.
            //metot şeklinde yazmak yerine içerisine lambda ile yazıyoruz, daha temiz bir yöntem.
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray()); //gelen mesajı byte'dan string'e çevirme işlemi
                Console.WriteLine("Gelen Mesaj: " + message);
            };

            Console.ReadLine();
          
        }

    }
}
