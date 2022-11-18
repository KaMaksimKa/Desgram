namespace Desgram.SharedKernel.Exceptions.BadRequestExceptions
{
    public class InvalidPasswordException:InvalidPropertyException
    {
        public override string PropertyName => "Password";
        public override string Message => "Вы ввели неверный пароль. Проверьте пароль и попробуйте ещё раз.";
    }
}
