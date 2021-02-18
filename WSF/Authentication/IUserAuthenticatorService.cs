namespace WSF.Authentication
{
    public interface IUserAuthenticatorService
    {
        bool TryAuthenticate(string username, string password);
    }
}