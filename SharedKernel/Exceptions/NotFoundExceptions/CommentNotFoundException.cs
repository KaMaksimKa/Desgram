using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class CommentNotFoundException : NotFoundException
    {
        public override string Message => "comment not found";

        public override string EntityName => "Comment";
    }
}
