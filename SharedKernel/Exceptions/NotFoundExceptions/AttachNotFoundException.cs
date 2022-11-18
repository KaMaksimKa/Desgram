using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class AttachNotFoundException : NotFoundException
    {
        public override string Message => "attach not found";

        public override string EntityName => "Attach";
    }
}
