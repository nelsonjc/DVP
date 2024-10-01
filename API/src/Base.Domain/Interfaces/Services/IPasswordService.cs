namespace TaskingSystem.Domain.Interfaces.Services
{
    public interface IPasswordService
    {
        bool CheckPassword(string hash, string password);
        string GenerateHash(string password);
    }
}
