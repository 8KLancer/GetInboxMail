using System;
using GmailAPIServices;
using System.Collections.Generic;

namespace GetInboxMail.Models
{    
    public class PageInfo
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; } 
        public int TotalItems { get; set; } 
        public int TotalPages  
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }

    public class IndexViewModel
    {
        public IEnumerable<GmailAPIService.CurrentMessage> Messages { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public class AuthModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}