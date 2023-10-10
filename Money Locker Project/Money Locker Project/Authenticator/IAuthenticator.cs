using MoneyLocker.Model;

namespace Money_Locker_Project.Authenticator
{
    public interface IAuthenticator
    {
        public ErrorInfo Validate(RequestValidator request);
    }
}
