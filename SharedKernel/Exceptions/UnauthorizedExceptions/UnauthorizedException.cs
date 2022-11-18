using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.UnauthorizedExceptions
{
    public class UnauthorizedException : Exception
    {
        public override string Message => "you are not authorize";
    }
}
