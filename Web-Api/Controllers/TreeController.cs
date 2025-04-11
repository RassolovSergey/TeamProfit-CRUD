using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Model;

namespace Web_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreeController : ControllerBase
    {
        private readonly TeamProfitDBContext _context;
        public TreeController(TeamProfitDBContext context)
        {
            _context = context;
        }

        // Загрузка корневых узлов – пользователей
        [HttpGet("root")]
        public async Task<IActionResult> GetRootNodes()
        {
            var users = await _context.Users.Include(u => u.Team).ToListAsync(); // Загружаем пользователей с командами
            var treeNodes = users.Select(u => new TreeNodeDto
            {
                Id = u.IdUser,
                Name = u.Login,
                NodeType = "User",
                HasChildren = _context.Costs.Any(c => c.IdUser == u.IdUser) // Проверяем наличие затрат
            }).ToList();

            return Ok(treeNodes);
        }

        // Ленивое получение дочерних узлов для User – Cost
        [HttpGet("user/{id}/children")]
        public async Task<IActionResult> GetUserChildren(int id)
        {
            List<TreeNodeDto> children = new List<TreeNodeDto>();

            var costs = await _context.Costs.Where(c => c.IdUser == id).ToListAsync();
            children = costs.Select(c => new TreeNodeDto
            {
                Id = c.IdCosts,
                Name = $"{c.TypeCosts} - {c.Amounts} руб.",
                NodeType = "Cost",
                HasChildren = false // Затраты не имеют дочерних узлов
            }).ToList();

            return Ok(children);
        }

        // Ленивое получение дочерних узлов для Team (пользователи в команде)
        [HttpGet("team/{id}/children")]
        public async Task<IActionResult> GetTeamChildren(int id)
        {
            List<TreeNodeDto> children = new List<TreeNodeDto>();

            var users = await _context.Users.Where(u => u.IdTeam == id).ToListAsync(); // Получаем пользователей для команды
            children = users.Select(u => new TreeNodeDto
            {
                Id = u.IdUser,
                Name = u.Login,
                NodeType = "User",
                HasChildren = _context.Costs.Any(c => c.IdUser == u.IdUser)
            }).ToList();

            return Ok(children);
        }

        // Ленивое получение дочерних узлов для Project (затраты по проекту)
        [HttpGet("project/{id}/children")]
        public async Task<IActionResult> GetProjectChildren(int id)
        {
            List<TreeNodeDto> children = new List<TreeNodeDto>();

            var costs = await _context.Costs.Where(c => c.IdProject == id).ToListAsync(); // Получаем затраты для проекта
            children = costs.Select(c => new TreeNodeDto
            {
                Id = c.IdCosts,
                Name = $"{c.TypeCosts} - {c.Amounts} руб.",
                NodeType = "Cost",
                HasChildren = false // Затраты не имеют дочерних узлов
            }).ToList();

            return Ok(children);
        }

        // Обновление родительской связи (drag & drop) для пользователей и затрат
        [HttpPut("update")]
        public async Task<IActionResult> UpdateNode([FromBody] UpdateNodeDto dto)
        {
            if (dto.NodeType.Equals("Cost", StringComparison.OrdinalIgnoreCase))
            {
                var cost = await _context.Costs.FindAsync(dto.NodeId);
                if (cost == null)
                    return NotFound();
                cost.IdProject = dto.NewParentId; // Обновляем проект для затрат
            }
            else
            {
                return BadRequest("Неверный тип узла для обновления.");
            }

            await _context.SaveChangesAsync();
            return Ok("Узел обновлён.");
        }

        // Обновление родительской связи для команды (перемещение пользователя между командами)
        [HttpPut("update-team")]
        public async Task<IActionResult> UpdateTeamNode([FromBody] UpdateNodeDto dto)
        {
            if (dto.NodeType.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                var user = await _context.Users.FindAsync(dto.NodeId);
                if (user == null)
                    return NotFound();
                user.IdTeam = dto.NewParentId; // Перемещаем пользователя в новую команду
            }
            else
            {
                return BadRequest("Неверный тип узла для обновления.");
            }

            await _context.SaveChangesAsync();
            return Ok("Узел обновлён.");
        }

        // Обновление родительской связи для проекта (перемещение затрат между проектами)
        [HttpPut("update-project")]
        public async Task<IActionResult> UpdateProjectNode([FromBody] UpdateNodeDto dto)
        {
            if (dto.NodeType.Equals("Cost", StringComparison.OrdinalIgnoreCase))
            {
                var cost = await _context.Costs.FindAsync(dto.NodeId);
                if (cost == null)
                    return NotFound();
                cost.IdProject = dto.NewParentId; // Перемещаем затраты в новый проект
            }
            else
            {
                return BadRequest("Неверный тип узла для обновления.");
            }

            await _context.SaveChangesAsync();
            return Ok("Узел обновлён.");
        }
    }
}
