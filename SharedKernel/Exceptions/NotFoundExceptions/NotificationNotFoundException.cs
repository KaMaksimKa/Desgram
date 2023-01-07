using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class NotificationNotFoundException:NotFoundException
    {
        public override string EntityName => "Notification";
        public override string Message => "notification not found";
    }
}
