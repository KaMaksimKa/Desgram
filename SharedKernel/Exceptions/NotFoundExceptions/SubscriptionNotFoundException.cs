using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class SubscriptionNotFoundException : NotFoundException
    {
        public override string Message => "subscription not found";

        public override string EntityName => "Subscription";
    }
}
