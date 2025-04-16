using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Model;
using Web_Api.Services;

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
            var teams = await _context.Teams.Include(t => t.Users).ToListAsync();
            var usersWithoutTeam = await _context.Users.Where(u => u.IdTeam == null).ToListAsync();

            var treeNodes = new List<TreeNodeDto>();

            // 1. Виртуальный корень "Без команды"
            if (usersWithoutTeam.Any())
            {
                treeNodes.Add(new TreeNodeDto
                {
                    Id = -1,
                    Name = "Без команды",
                    NodeType = "VirtualTeam",
                    HasChildren = true,
                    ParentId = 0,
                    ParentType = "None"
                });
            }

            // 2. Реальные команды
            treeNodes.AddRange(teams.Select(team => new TreeNodeDto
            {
                Id = team.Id,
                Name = team.Name,
                NodeType = "Team",
                HasChildren = team.Users != null && team.Users.Any(),
                ParentId = 0,
                ParentType = "None"
            }));

            return Ok(treeNodes);
        }


        // Ленивое получение дочерних узлов
        [HttpGet("{nodeType}/{id}/children")]
        public async Task<IActionResult> GetChildren(string nodeType, int id)
        {
            List<TreeNodeDto> children = new();

            switch (nodeType)
            {
                case "Team":
                    var users = await _context.Users.Where(u => u.IdTeam == id).ToListAsync();
                    children = users.Select(u => new TreeNodeDto
                    {
                        Id = u.Id,
                        Name = u.Login,
                        NodeType = "User",
                        HasChildren = _context.Costs.Any(c => c.IdUser == u.Id),
                        ParentId = id,
                        ParentType = "Team"
                    }).ToList();
                    break;

                case "User":
                    var costs = await _context.Costs.Where(c => c.IdUser == id).ToListAsync();
                    children = costs.Select(c => new TreeNodeDto
                    {
                        Id = c.Id,
                        Name = $"{c.Amounts} ₽",
                        NodeType = "Cost",
                        HasChildren = false,
                        ParentId = id,
                        ParentType = "User"
                    }).ToList();
                    break;

                default:
                    return BadRequest("Неизвестный тип узла.");
            }

            return Ok(children);
        }

        // Удаление узлов дерева с контролем целостности
        [HttpDelete("{nodeType}/{id}")]
        public async Task<IActionResult> DeleteNode(string nodeType, int id)
        {
            switch (nodeType)
            {
                case "Cost":
                    var cost = await _context.Costs.FindAsync(id);
                    if (cost == null) return NotFound();
                    _context.Costs.Remove(cost);
                    await _context.SaveChangesAsync();
                    return NoContent();

                case "User":
                    var user = await _context.Users.FindAsync(id);
                    if (user == null) return NotFound();

                    bool hasCosts = await _context.Costs.AnyAsync(c => c.IdUser == id);
                    if (hasCosts)
                        return BadRequest("Невозможно удалить пользователя, у которого есть затраты.");

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return NoContent();

                case "Team":
                    if (id == 0)
                        return BadRequest("Команду 'Без команды' удалить нельзя.");

                    var team = await _context.Teams.FindAsync(id);
                    if (team == null) return NotFound();

                    bool hasUsers = await _context.Users.AnyAsync(u => u.IdTeam == id);
                    if (hasUsers)
                        return BadRequest("Невозможно удалить команду, в которой есть пользователи.");

                    _context.Teams.Remove(team);
                    await _context.SaveChangesAsync();
                    return NoContent();

                default:
                    return BadRequest("Неизвестный тип узла.");
            }
        }

        // Создаие узлов дерева
        [HttpPost]
        public async Task<IActionResult> CreateNode([FromBody] CreateNodeDto dto)
        {
            switch (dto.NodeType)
            {
                case "Team":
                    if (string.IsNullOrWhiteSpace(dto.Name))
                        return BadRequest("Для команды нужно указать имя.");

                    var team = new Team { Name = dto.Name };
                    _context.Teams.Add(team);
                    await _context.SaveChangesAsync();
                    return Ok(new { team.Id });

                case "User":
                    if (string.IsNullOrWhiteSpace(dto.Name) ||
                        string.IsNullOrWhiteSpace(dto.Email) ||
                        string.IsNullOrWhiteSpace(dto.Password))
                        return BadRequest("Для пользователя необходимо указать имя, email и пароль.");

                    var salt = PasswordHelper.GenerateSalt();
                    var hash = PasswordHelper.ComputeHash(dto.Password, salt);

                    var user = new User
                    {
                        Login = dto.Name,
                        Email = dto.Email,
                        PasswordHash = hash,
                        Salt = salt,
                        IsActive = true,
                        PriceWork = 10,
                        DateRegistration = DateTime.UtcNow,
                        IdTeam = dto.ParentId == 0 ? null : dto.ParentId
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok(new { user.Id });

                case "Cost":
                    if (dto.Amount == null || dto.ParentId == null)
                        return BadRequest("Необходимо указать сумму и ID пользователя.");

                    var cost = new Cost
                    {
                        Amounts = dto.Amount.Value,
                        IdUser = dto.ParentId.Value
                    };

                    _context.Costs.Add(cost);
                    await _context.SaveChangesAsync();
                    return Ok(new { cost.Id });

                default:
                    return BadRequest("Неизвестный тип узла.");
            }
        }

        // Обновление узлов дерева
        [HttpPut]
        public async Task<IActionResult> UpdateNode([FromBody] UpdateNodeDto dto)
        {
            switch (dto.NodeType)
            {
                case "Team":
                    if (dto.Id == 0)
                        return BadRequest("Системную команду 'Без команды' нельзя редактировать.");

                    if (string.IsNullOrWhiteSpace(dto.Name))
                        return BadRequest("Имя команды не может быть пустым.");

                    var team = await _context.Teams.FindAsync(dto.Id);
                    if (team == null) return NotFound();

                    team.Name = dto.Name;
                    await _context.SaveChangesAsync();
                    return NoContent();

                case "User":
                    if (string.IsNullOrWhiteSpace(dto.Name))
                        return BadRequest("Имя пользователя не может быть пустым.");

                    var user = await _context.Users.FindAsync(dto.Id);
                    if (user == null) return NotFound();

                    user.Login = dto.Name;
                    await _context.SaveChangesAsync();
                    return NoContent();

                case "Cost":
                    if (dto.Amount == null)
                        return BadRequest("Необходимо указать сумму.");

                    var cost = await _context.Costs.FindAsync(dto.Id);
                    if (cost == null) return NotFound();

                    cost.Amounts = dto.Amount.Value;
                    await _context.SaveChangesAsync();
                    return NoContent();

                default:
                    return BadRequest("Неизвестный тип узла.");
            }
        }
    }
}
