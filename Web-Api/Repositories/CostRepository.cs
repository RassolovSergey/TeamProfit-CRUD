using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Model;
using Web_Api.Repositories.Interfaces;

namespace Web_Api.Repositories
{
    public class CostRepository : ICostRepository
    {
        private readonly TeamProfitDBContext _context;

        public CostRepository(TeamProfitDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cost>> GetAllAsync()
        {
            return await _context.Costs.Include(c => c.User).ToListAsync();
        }

        public async Task<Cost?> GetByIdAsync(int id)
        {
            return await _context.Costs.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Cost cost)
        {
            await _context.Costs.AddAsync(cost);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cost cost)
        {
            _context.Costs.Update(cost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cost = await _context.Costs.FindAsync(id);
            if (cost != null)
            {
                _context.Costs.Remove(cost);
                await _context.SaveChangesAsync();
            }
        }
    }
}
