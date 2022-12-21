using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class BadRequestException:Exception
    {
        public Dictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();

        public BadRequestException()
        {
            
        }
        public BadRequestException(Dictionary<string, List<string>> errors)
        {
            Errors = errors;
        }
    }

   
}
