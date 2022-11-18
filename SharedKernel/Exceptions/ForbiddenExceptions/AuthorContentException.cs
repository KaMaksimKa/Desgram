using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.ForbiddenExceptions
{
    public class AuthorContentException:ForbiddenException
    {
        public override string Message => "you are not the author of the content";
    }
}
