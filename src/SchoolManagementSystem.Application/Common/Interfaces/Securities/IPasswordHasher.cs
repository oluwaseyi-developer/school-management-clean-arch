namespace SchoolManagementSystem.Application.Common.Interfaces.Securities
{
    public interface IPasswordHasher
    {
        void CreatePassword(string password, out byte[] hash, out byte[] salt);

        bool VerifyPassword(string password, byte[] hash, byte[] salt);
    }
}
