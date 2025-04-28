using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.Notification
{
    public class Notification : FullAuditedAggregateRoot<Guid>
    {
        public Guid ReceiverId { get; set; }
        public string Template { get; set; }
    }
}
