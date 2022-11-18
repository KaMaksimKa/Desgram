using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public override string Message => "user not found";
        public override string EntityName => "User";
    }
}
