using System.Collections.Generic;
using AE.Net.Mail;
using GmailAPIServices;


namespace INboxImapServices
{
    public class INboxImapService
    {
        public List<GmailAPIService.CurrentMessage> GetAllMessages(string mail, string password)
        {
            List<GmailAPIService.CurrentMessage> messagesInfo = new List<GmailAPIService.CurrentMessage>();
            
             ImapClient ic = new ImapClient("imap.gmail.com", mail, password,
                             AuthMethods.Login, 993, true);

             ic.SelectMailbox("INBOX");

             int messageCount = ic.GetMessageCount();

             MailMessage[] mm = ic.GetMessages(messageCount, messageCount - 30);
             foreach (MailMessage m in mm)
             {

                 messagesInfo.Add(new GmailAPIService.CurrentMessage()
                 {
                     Id = m.MessageID.Replace('<', '"').Replace('>', '"'),
                     Date = m.Date.ToString(),
                     From = m.From.ToString(),
                     Subject = m.Subject
                 });
             }
             ic.Dispose();
            return messagesInfo;
        }        
    }
}
