using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class InvalidUserIdException:InvalidPropertyException
    {
        public override string PropertyName => "UserId";
        public override string Message => "unexpected users' id";
    }
}
