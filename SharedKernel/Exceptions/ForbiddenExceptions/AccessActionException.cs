namespace Desgram.SharedKernel.Exceptions.ForbiddenExceptions
{
    public class AccessActionException:ForbiddenException
    {
        public override string Message => "you don't have enough authority to perform this action";
    }
}
