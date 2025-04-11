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

        // POST: api/Tree/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateNode([FromBody] TreeNodeDto nodeDto)
        {
            if (nodeDto == null)
                return BadRequest("Неверные данные узла.");

            // Пример: создание узла типа "User"
            if (nodeDto.NodeType.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                // Создаем новую сущность пользователя. Предполагается, что модель User содержит минимум поля IdUser и Login.
                var user = new User
                {
                    Login = nodeDto.Name
                    // Можно добавить другие свойства, если они требуются (например, Email и т.д.)
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Формируем возвращаемый узел на основании созданного пользователя.
                var createdNode = new TreeNodeDto
                {
                    Id = user.IdUser,
                    Name = user.Login,
                    NodeType = "User",
                    HasChildren = false // Новый пользователь сначала не имеет дочерних узлов
                };
                return Ok(createdNode);
            }
            else if (nodeDto.NodeType.Equals("Cost", StringComparison.OrdinalIgnoreCase))
            {
                // Если поддерживается создание затрат, аналогичный блок можно реализовать
                // Пример (учтите, что для затрат могут понадобиться дополнительные поля):
                // var cost = new Cost { Amounts = ..., TypeCosts = ..., IdUser = ..., ... };
                // _context.Costs.Add(cost); await _context.SaveChangesAsync();
                // var createdNode = new TreeNodeDto { Id = cost.IdCosts, Name = $"{cost.TypeCosts} - {cost.Amounts} руб.", NodeType = "Cost", HasChildren = false };
                // return Ok(createdNode);
                return BadRequest("Создание узлов типа 'Cost' не поддерживается в данной версии.");
            }
            else
            {
                return BadRequest("Неверный тип узла для создания.");
            }
        }

    }
}
