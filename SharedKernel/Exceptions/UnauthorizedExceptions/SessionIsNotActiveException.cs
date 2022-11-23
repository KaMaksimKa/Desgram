namespace Desgram.SharedKernel.Exceptions.UnauthorizedExceptions
{
    public class SessionIsNotActiveException:UnauthorizedException
    {
        public override string Message => "session is not active";
        public override string Code => "session_is_not_active";
    }
}
