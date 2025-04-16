using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Model;
using Web_Api.Repositories.Interfaces;

namespace Web_Api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TeamProfitDBContext _context;
        public AuthRepository(TeamProfitDBContext context) => _context = context;

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
