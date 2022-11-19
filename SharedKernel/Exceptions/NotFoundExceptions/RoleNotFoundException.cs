using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class RoleNotFoundException:NotFoundException
    {
        public override string EntityName => "ApplicationRole";
        public override string Message => "role not found";
    }
}
