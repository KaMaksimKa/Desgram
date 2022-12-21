using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class EmailCodeNotFoundException : NotFoundException
    {
        public override string Message => "email code not found";
        public override string EntityName => "EmailCode";
    }
}
