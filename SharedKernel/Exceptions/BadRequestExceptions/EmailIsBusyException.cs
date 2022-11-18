using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class EmailIsBusyException:InvalidPropertyException
    {
        public override string PropertyName => "Email";
        public override string Message => "Этот электронный адрес уже используется в другом аккаунте.";
    }
}
