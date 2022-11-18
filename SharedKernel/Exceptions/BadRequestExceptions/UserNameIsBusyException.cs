using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class UserNameIsBusyException:InvalidPropertyException
    {
        public override string PropertyName => "UserName";
        public override string Message => "Это имя пользователя уже используется в другом аккаунте.";
    }
}
