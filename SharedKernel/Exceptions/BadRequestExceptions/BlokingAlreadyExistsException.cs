using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class BlockingAlreadyExistsException : EntityAlreadyExistsException
    {
        public override string EntityName => "Blocking";
        public override string Message => "you've already blocked";
    }
}
