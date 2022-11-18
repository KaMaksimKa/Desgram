using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class PostNotFoundException : NotFoundException
    {
        public override string Message => "post not found";

        public override string EntityName => "Post";
    }
}
