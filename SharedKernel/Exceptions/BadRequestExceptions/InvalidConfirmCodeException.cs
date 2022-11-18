using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class ConfirmCodeHasExpiredException:InvalidPropertyException
    {
        public override string PropertyName => "ConfirmCode";
        public override string Message => "Время действия данного кода истекло, повторите операцию заново";
    }
}
