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
            public CurrentMessage()
            {
            }
            public CurrentMessage(string _id, string _from, string _date, string _subject)
            {
                Id = _id;
                From = _from;
                Date = _date;
                Subject = _subject;
            }
        }

        public async Task<IEnumerable<CurrentMessage>> GetRangeMessagesAsync(int skip, int take)
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
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });


            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest labelsRequest = service.Users.Labels.List("me");
            List<Message> messages = await ListMessages(service, "me", "");
            MessagesCount = messages.Count();
            messages = messages.Skip(skip * take).Take(take).ToList();
            
            List<CurrentMessage> messagesInfo = new List<CurrentMessage>();
            if (messages != null)
            {
                foreach (var msg in messages)
                {
                    var emailInfoRequest = service.Users.Messages.Get("me", msg.Id);
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

                            // get message body
                            //if (emailInfoResponse.Payload.Parts == null && emailInfoResponse.Payload.Body != null)
                            //    body = DecodeBase64String(emailInfoResponse.Payload.Body.Data);
                            //else
                            //    body = GetNestedBodyParts(emailInfoResponse.Payload.Parts, "");

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
                try
                {
                    ListMessagesResponse response = await request.ExecuteAsync();
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }

        //public  static String DecodeBase64String(string s)
        //{
        //    var ts = s.Replace("-", "+");
        //    ts = ts.Replace("_", "/");
        //    var bc = Convert.FromBase64String(ts);
        //    var tts = Encoding.UTF8.GetString(bc);

        //    return tts;
        //}

        //private String GetNestedBodyParts(IList<MessagePart> part, string curr)
        //{
        //    string str = curr;
        //    if (part == null)
        //    {
        //        return str;
        //    }
        //    else
        //    {
        //        foreach (var parts in part)
        //        {
        //            if (parts.Parts == null)
        //            {
        //                if (parts.Body != null && parts.Body.Data != null)
        //                {
        //                    var ts = DecodeBase64String(parts.Body.Data);
        //                    str += ts;
        //                }
        //            }
        //            else
        //            {
        //                return GetNestedBodyParts(parts.Parts, str);
        //            }
        //        }

        //        return str;
        //    }
        //}

    }

}

