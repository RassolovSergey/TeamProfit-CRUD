using Microsoft.AspNetCore.Mvc;
using Web_Api.Model;
using Web_Api.Repositories.Interfaces;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostController : ControllerBase
    {
        private readonly ICostRepository _repository;

        public CostController(ICostRepository repository)
        {
            _repository = repository;
        }

        // GetAll
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cost>>> GetAll()
        {
            var costs = await _repository.GetAllAsync();
            return Ok(costs);
        }

        // Get {Id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cost>> Get(int id)
        {
            var cost = await _repository.GetByIdAsync(id);
            if (cost == null) return NotFound();
            return Ok(cost);
        }

        // Create {newCost}
        [HttpPost]
        public async Task<IActionResult> Create(Cost cost)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.AddAsync(cost);
            return CreatedAtAction(nameof(Get), new { id = cost.Id }, cost);
        }

        // Update {Id, newCost}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cost updatedCost)
        {
            if (id != updatedCost.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _repository.UpdateAsync(updatedCost);
            return NoContent();
        }

        // Delete {Id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
