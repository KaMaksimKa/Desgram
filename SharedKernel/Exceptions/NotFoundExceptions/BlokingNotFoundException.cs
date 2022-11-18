using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class BlockingNotFoundException : NotFoundException
    {
        public override string Message => "blocking not found";

        public override string EntityName => "Blocking";
    }
}
