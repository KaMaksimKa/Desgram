using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class UnconfirmedEmailNotFoundException : NotFoundException
    {
        public override string Message => "unconfirmed email not found";

        public override string EntityName => "UnconfirmedEmail";
    }
}
