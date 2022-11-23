namespace Desgram.SharedKernel.Exceptions.UnauthorizedExceptions
{
    public class InvalidRefreshTokenException:UnauthorizedException
    {
        public override string Code => "invalid_refresh_token";
        public override string Message => " invalid refresh token";
    }
}
