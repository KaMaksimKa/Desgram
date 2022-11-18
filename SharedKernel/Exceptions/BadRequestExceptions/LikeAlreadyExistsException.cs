using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class LikeAlreadyExistsException:EntityAlreadyExistsException
    {
        public override string EntityName => "Like";
        public override string Message => "you've already liked";
    }
}
