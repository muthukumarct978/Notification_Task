using System;
using System.Collections.Generic;
using System.Text;

namespace InterviewFalaina.Model
{
    public class NotificationModel
    {
        public string to { get; set; }
        public Notification notification { get; set; }
    }
    public class Notification
    {
        public string body { get; set; }
        public string title { get; set; }
    }
    public class ResponseModel
    {
        public string resp;
    }
}