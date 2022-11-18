using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class SubscriptionAlreadyExistsException:EntityAlreadyExistsException
    {
        public override string EntityName => "Subscription";
        public override string Message => "you've already following";
    }
}
