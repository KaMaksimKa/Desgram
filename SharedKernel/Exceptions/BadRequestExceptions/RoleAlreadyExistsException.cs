using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class RoleAlreadyExistsException:EntityAlreadyExistsException
    {
        public override string EntityName => "ApplicationRole";
        public override string Message => "role already exists";
    }
}
