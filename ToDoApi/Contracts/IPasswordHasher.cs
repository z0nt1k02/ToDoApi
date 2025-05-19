namespace ToDoApi.Contracts;

public interface IPasswordHasher
{
    string GenerateHash(string password);
    bool VerifyHash(string hash, string password);
}