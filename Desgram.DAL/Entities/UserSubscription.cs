using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.DAL.Entities
{
    public class UserSubscription
    {
        public Guid Id { get; set; }

        public Guid SubscriberId { get; set; }
        public User? Subscriber { get; set; }

        public Guid SubscriptionId { get; set; }
        public User? Subscription { get; set; }
    }
}
