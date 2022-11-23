using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class FileFormatIsNotSupportedException:InvalidPropertyException
    {
        public override string PropertyName => "MimeType";
        public override string Message => "This file format is not supported";
    }
}
