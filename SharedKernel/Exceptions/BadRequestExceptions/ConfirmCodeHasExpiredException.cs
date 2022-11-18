using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class InvalidConfirmCodeException:InvalidPropertyException
    {
        public override string PropertyName => "ConfirmCode";
        public override string Message => "Данный код неверен, проверьте его или отправьте новый.";
    }
}
