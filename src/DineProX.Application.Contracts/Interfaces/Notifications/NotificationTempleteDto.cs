using System;
using System.Collections.Generic;
using System.Text;

namespace DineProX.Interfaces.Notifications
{
    public class NotificationTempleteDto
    {
        public string Title { get; set; }
        public string body { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
    }
}
