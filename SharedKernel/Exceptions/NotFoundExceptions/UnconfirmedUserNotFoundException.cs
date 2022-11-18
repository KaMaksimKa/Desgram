using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class UnconfirmedUserNotFoundException : NotFoundException
    {
        public override string Message => "unconfirmed user not found";
        public override string EntityName => "UnconfirmedUser";
    }
}
