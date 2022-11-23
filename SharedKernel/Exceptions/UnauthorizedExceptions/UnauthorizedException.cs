namespace Desgram.SharedKernel.Exceptions.UnauthorizedExceptions
{
    public class UnauthorizedException : Exception
    {
        public virtual string Code => "not_authorize";
        public override string Message => "you are not authorize";
    }
}
