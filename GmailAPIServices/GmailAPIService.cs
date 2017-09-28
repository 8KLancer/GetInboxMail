using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GmailAPIServices
{
    public class GmailAPIService
    {
        public int MessagesCount { get; private set; }
        private string[] Scopes = { GmailService.Scope.GmailReadonly };
        private string ApplicationName = "Google Mail";

        public class CurrentMessage
        {
            public string Id { get; set; }
            public string From { get; set; }
            public string Date { get; set; }
            public string Subject { get; set; }
            public CurrentMessage() { }

            public CurrentMessage(string _id, string _from, string _date, string _subject)
            {
                Id = _id;
                From = _from;
                Date = _date;
                Subject = _subject;
            }
        }

        private GmailService CreateGmailService()
        {
            UserCredential credential;
            string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            GmailService service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

        public async Task<IEnumerable<CurrentMessage>> GetRangeMessagesAsync(int skip, int take)
        {
            GmailService service = CreateGmailService();

            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest labelsRequest = service.Users.Labels.List("me");
            List<Message> messages = await ListMessages(service, "me", "");
            MessagesCount = messages.Count();
            messages = messages.Skip(skip * take).Take(take).ToList();
            
            List<CurrentMessage> messagesInfo = await GetMessagesPartsAsync(messages, service);
            return messagesInfo;
        }

        private async Task<List<CurrentMessage>> GetMessagesPartsAsync(List<Message> messages, GmailService service)
        {
            List<CurrentMessage> messagesInfo = new List<CurrentMessage>();
            if (messages != null)
            {
                foreach (var msg in messages)
                {
                    UsersResource.MessagesResource.GetRequest emailInfoRequest = service.Users.Messages.Get("me", msg.Id);
                    var emailInfoResponse = await emailInfoRequest.ExecuteAsync();

                    if (emailInfoResponse != null)
                    {
                        CurrentMessage currentMsg = new CurrentMessage();
                        foreach (var mParts in emailInfoResponse.Payload.Headers)
                        {
                            currentMsg.Id = msg.Id;
                            if (mParts.Name == "Date")
                            {
                                currentMsg.Date = mParts.Value;
                            }
                            else if (mParts.Name == "From")
                            {
                                currentMsg.From = mParts.Value;
                            }
                            else if (mParts.Name == "Subject")
                            {
                                currentMsg.Subject = mParts.Value;
                            }
                        }
                        messagesInfo.Add(currentMsg);
                    }
                }
            }
            return messagesInfo;
        }

        private async Task<List<Message>> ListMessages(GmailService service, String userId, String query)
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
            // disable spam and trash
            request.IncludeSpamTrash = false;
            //choosing INBOX option
            request.LabelIds = "INBOX";
            request.Q = query;

            do
            {                
                    ListMessagesResponse response = await request.ExecuteAsync();
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
                
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }        
    }
}

