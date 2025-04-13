using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Model;
using Web_Api.Repositories.Interfaces;

namespace Web_Api.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamProfitDBContext _context;

        public TeamRepository(TeamProfitDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Teams.Include(t => t.Users).ToListAsync();
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams.Include(t => t.Users).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }
        }
    }

}
