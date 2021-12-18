using System.Runtime.Remoting.Messaging;

namespace WebBalanceTracker
{
    internal class MessagesWorker : BackgroundWorkerBase
    {
        public override int RepeatEvery => 30 * 1000; // 30 seconds;


        public override void DoWork()
        {
            
            try
            {
                //var messagesToBeSent = Db.GetData<I_Message>(new SearchParameters { IMessageSendMail = true });
                //foreach (var messageToBeSent in messagesToBeSent)
                //{
                //    var title = $"{messageToBeSent.IType} : {messageToBeSent.Title}";
                //    var body = $"{messageToBeSent.Message} {System.Environment.NewLine} {messageToBeSent.ExtraData}";
                //    if(EmailHelper.SendMail(title, body))
                //    {
                //        messageToBeSent.SendMail = false;
                //        Db.Update(messageToBeSent);
                //    }
                //}
            }
            catch (System.Exception e)
            {
                
            }
        }
    }
}