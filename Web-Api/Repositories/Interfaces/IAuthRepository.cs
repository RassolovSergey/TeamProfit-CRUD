using Web_Api.Model;

namespace Web_Api.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task AddUserAsync(User user);
    }

}
