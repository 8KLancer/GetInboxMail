using GetInboxMail.Models;
using GmailAPIServices;
using INboxImapServices;
using System.Collections.Generic;
using System.Web.Http;

namespace GetInboxMail.Controllers
{
    public class ImapServiceController : ApiController
    {
        INboxImapService service = new INboxImapService();

        public IEnumerable<GmailAPIService.CurrentMessage> Post(AuthModel model)
        {
            IEnumerable<GmailAPIService.CurrentMessage> messages = service.GetAllMessages(model.Login, model.Password);
            return messages;
        }        
    }
}
