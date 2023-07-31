using Microsoft.Extensions.Configuration;
using MSMQ.Messaging;
using System.Linq.Expressions;

namespace MSMQTest
{
    public class MSMQWorkQueueWriter
    {
        public class LogMessage

        {

            public string MessageText { get; set; }

            public DateTime MessageTime { get; set; }

        }
        public static void WriteMessage()
        {
            string messageString = "test message";
            string queuePath = ".\\private$\\ackqueue";


            try
            {
                if (!MessageQueue.Exists(queuePath))
                {
                    MessageQueue.Create(queuePath, true);
                }
                using (var queue = new MessageQueue(queuePath))
                {
                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogMessage) });

                    queue.Send(messageString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }


        public static Task ReadItem()
        {
            try
            {

                string queuePath = ".\\private$\\ackqueue";
                MessageQueue queue = new(queuePath)
                {
                    Formatter = new XmlMessageFormatter(new Type[] { typeof(string) })
                };

                Message message = queue.Receive();
                return Task.FromResult(message.ToString());

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
