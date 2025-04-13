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

        // Ленивое получение дочерних узлов для User – Cost, 
        [HttpGet("{nodeType}/{id}/children")]
        public async Task<IActionResult> GetUserChildren(int id)
        {
            List<TreeNodeDto> children = new List<TreeNodeDto>();

            var costs = await _context.Costs.Where(c => c.IdUser == id).ToListAsync();
            children = costs.Select(c => new TreeNodeDto
            {
                Id = c.IdCosts,
                Name = $"{c.IdCosts} - {c.Amounts} руб.",
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
    }
}
