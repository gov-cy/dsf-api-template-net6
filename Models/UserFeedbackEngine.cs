using System;
using System.Collections.Generic;

namespace dsf_api_template_net6.Models
{
    public class UserFeedbackEngineRequest
    {
        public string PageSource { get; set; }
        public string SessionId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        //public string HashedId { get; set; }
    }

    public class UserFeedbackRecords
    {
        public int RecordsFound { get; set; }
        public string Info {get; set;}
        public List<UserFeedbackRecord> FeedbackRecords { get; set; }
    }
    public class UserFeedbackRecord
    {
        public int Uid { get; set;}
        public string ServiceId { get; set;}
        public string ClientId { get; set; }        
        public string PageSource { get; set; }
        public string SessionId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string HashId { get; set; }
    }
}
