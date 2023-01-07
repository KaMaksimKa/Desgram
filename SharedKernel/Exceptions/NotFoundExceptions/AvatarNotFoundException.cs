using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class AvatarNotFoundException : NotFoundException
    {
        public override string Message => "avatar not found";

        public override string EntityName => "Avatar";
    }
}
