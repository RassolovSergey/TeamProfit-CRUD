using Microsoft.AspNetCore.Mvc;
using Web_Api.Model;
using Web_Api.Repositories.Interfaces;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _repository;

        public TeamController(ITeamRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetAll()
        {
            var teams = await _repository.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> Get(int id)
        {
            var team = await _repository.GetByIdAsync(id);
            if (team == null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Team team)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Возвращаем ошибки валидации

            await _repository.AddAsync(team);
            return CreatedAtAction(nameof(Get), new { id = team.Id }, team);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Team updatedTeam)
        {
            if (id != updatedTeam.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _repository.UpdateAsync(updatedTeam);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
