using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Model;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostController : ControllerBase
    {
        private readonly TeamProfitDBContext _context;

        public CostController(TeamProfitDBContext context)
        {
            _context = context;
        }

        // GET: api/Cost
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cost>>> GetCosts()
        {
            return await _context.Costs.Include(c => c.User).ToListAsync();
        }

        // GET: api/Cost
        [HttpGet("{id}")]
        public async Task<ActionResult<Cost>> GetCost(int id)
        {
            var cost = await _context.Costs.Include(c => c.User).FirstOrDefaultAsync(c => c.IdCosts == id);

            if (cost == null)
            {
                return NotFound();
            }

            return cost;
        }

        // POST: api/Cost
        [HttpPost]
        public async Task<ActionResult<Cost>> PostCost(Cost cost)
        {
            _context.Costs.Add(cost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCost", new { id = cost.IdCosts }, cost);
        }

        // PUT: api/Cost/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCost(int id, Cost cost)
        {
            if (id != cost.IdCosts)
            {
                return BadRequest();
            }

            _context.Entry(cost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Cost/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCost(int id)
        {
            var cost = await _context.Costs.FindAsync(id);
            if (cost == null)
            {
                return NotFound();
            }

            _context.Costs.Remove(cost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CostExists(int id)
        {
            return _context.Costs.Any(e => e.IdCosts == id);
        }
    }
}
