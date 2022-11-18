using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class LikeNotFoundException : NotFoundException
    {
        public override string Message => "like not found";

        public override string EntityName => "Like";
    }
}
