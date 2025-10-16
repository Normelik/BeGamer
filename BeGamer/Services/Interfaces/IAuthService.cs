namespace BeGamer.Services.Interfaces
{
    public interface IAuthService
    {

        string Login(string username, string password);
        string Register(string username, string password);
    }
}
