using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public class SessionNotFoundException:NotFoundException
    {
        public override string Message => "session not found"; 
        public override string EntityName => "Session";

    }
}
