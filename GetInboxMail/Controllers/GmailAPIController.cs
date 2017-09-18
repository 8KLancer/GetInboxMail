using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GmailAPIServices;
using System.Threading.Tasks;
using GetInboxMail.Models;
using Newtonsoft.Json;

namespace GetInboxMail.Controllers
{
    public class GmailAPIController : ApiController
    {
        GmailAPIService service = new GmailAPIService();
        private const int pageSize = 10; // количество объектов на страницу

        public async Task<IEnumerable<GmailAPIService.CurrentMessage>> Post([FromBody]int pageNumberTO)
        {
            IEnumerable<GmailAPIService.CurrentMessage> messages = await service.GetRangeMessagesAsync(pageNumberTO - 1, pageSize);
            return messages;
        }

        public async Task<IndexViewModel> Get()
        {
            IEnumerable<GmailAPIService.CurrentMessage> messages = await service.GetRangeMessagesAsync(0,pageSize);

            int messagesCount = service.MessagesCount;
            PageInfo pageInfo = new PageInfo { PageNumber = 1, PageSize = pageSize, TotalItems = messagesCount };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Messages = messages };

            return ivm;
        }
    }
}
