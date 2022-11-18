using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.NotFoundExceptions
{
    public abstract class NotFoundException : Exception
    {
        public abstract string EntityName { get; }
    }
}
