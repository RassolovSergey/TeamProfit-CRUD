using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Model;
using Web_Api.Repositories.Interfaces;

namespace Web_Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TeamProfitDBContext _context;

        public UserRepository(TeamProfitDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Team).Include(u => u.Costs).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.Include(u => u.Team).Include(u => u.Costs).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }

}
