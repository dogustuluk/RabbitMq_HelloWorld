using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //publisher >> producer olarak da adlandırılabilir
            //RabbitMQ ile haberleşebilmek için dependencies'e "RabbitMQ.Client" paketini yüklememiz gerekir.
            var factory = new ConnectionFactory(); //rabbitMQ'ya bağlanmak için bir "Connection Class" oluşturmamız lazım.
            factory.Uri = new Uri("amqps://nxdranwu:n_3nr-xZlXx0NoCWuFP05gTqZfp7_hwK@sparrow.rmq.cloudamqp.com/nxdranwu ");
            //uri'yi rabbit cloud'taki -CloudAMQP- instance'ımızın içerisinden buluyoruz. AMQP URL'i buraya yapıştırıyoruz.
            //gerçek hayat senaryolarında buraya uri'yi direkt olarak yapıştırmayız, bu bilgileri appSetting.cs'in içerisinde tutarız.
            //uri yazıldıktan sonra rabbitMQ ile bağlantı sağlamış oluyoruz.
            //------
            //şimdi bağlantı açmamız gerekmektedir. using ifadesi ile açıldı.
            using var connection = factory.CreateConnection();  //connection Main scope'u bitince düşmektedir.

            //bağlantı açıldı. Şimdi rabbitMQ'ya bu bağlantı aracılığıyla bir kanal ile bağlanmamız gerekiyor. 
            var channel = connection.CreateModel();  //kanal oluştu. Artık bu kanal üzerinden rabbitMQ ile haberleşilecektir.

            //rabbitMQ'ya bir mesaj yollandığında bir kuyruğun olması lazım. Eğer yoksa mesajlar boşa gider.
            //ilk olarak kuyruk oluşturmak zorunludur.
            channel.QueueDeclare("hello-queue", true, false, false);
            //QueueDeclare yapısı >>> ("kuyruğun ismi",durable,exclusive,autoDelete)
            //Durable >>> false olduğunda kuyruklar memory'de tutulur ve rabbit'e reset atıldığında kuyruk silinir,
                    //true olduğunda ise kuyruk fiziksel olarak tutulur ve rabbit'e reset atılsa dahi kuyruk silinmez.
            //exclusive >>> exclusive property'sini true yaparsak sadece burada oluşturduğumuz kanal üzerinden bağlanabiliriz,
                    //false olduğunda ise farklı bir kanal üzerinden de bu kuyruğa erişim sağlayabiliriz. Subscriber'daki
                    //bir kuyruğun da buna erişmesi gerekecektir. Genellikle false olmalıdır.
            //autoDelete >>> eğer kuyruğa bağlı olan son subscriber bağlantısını koparırsa kuyruğu otomatik olarak siler. bu pek
                    //istenen bir durum değildir. Çünkü subscriber yanlışlıkla silinirse kuyruk da silinir.
            var message = "Hello World!"; //mesaj oluştu.
            //rabbitMQ'ya mesajlar byte dizini olarak gönderilir.
            //byte olarak gönderilmesinin avantajları >> pdf, image veya büyük bir dosya gönderimine olanak sağlar.

            var messageBody = Encoding.UTF8.GetBytes(message); //mesaj byte dizisine çevrildi.

            //mesajı kanala yolluyoruz.
            channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
            //BasicPublish Yapısı >>> (string exchange,routingKey)
            //string exchange >>> exchange kullanmadığımız için direkt olarak kuyruğa "string.Empty" diyerek yolluyoruz.
                    //eğer exchange kullanmadan yollanan işleme default-exchange denir.
            //routingKey >>> default-exchange kullanıyor isek routingKey'imize kuyruğun adını vermemiz gerekmektedir.
            //body >>> byte dizisi şeklinde message body'i gönderiyoruz.

            //bağlantı (connection) oluşturmak pahalı bir işlemdir, bir bağlantı üzerinde birden fazla kanal ile haberleşmemiz gerekmektedir.
            //bu durumu sağlayan ise rabbitMQ'dur.
            Console.WriteLine("Mesaj Gönderilmiştir");

            Console.ReadLine();
            
        }
    }
}
