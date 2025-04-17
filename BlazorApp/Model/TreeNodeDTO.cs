using Microsoft.VisualBasic;

namespace BlazorApp.Model
{
    public class TreeNodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NodeType { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string? ParentType { get; set; }
        public bool HasChildren { get; set; }
        public List<TreeNodeDto> Children { get; set; } = new();
    }
    public class UpdateNodeDto
    {
        public string NodeType { get; set; } = null!;
        public int Id { get; set; }

        public string? Name { get; set; }       // для команды и пользователя
        public decimal? Amount { get; set; }    // для затрат
    }
    // Скрываем: ХешПороля и его Соль
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal PriceWork { get; set; } = 10.0m;
        public int IdTeam { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateAndTime DateRegistration { get; set; }
    }
    public class TeamDTO
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }
    public class CostDTO
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public decimal? Amounts { get; set; } = 10.0m;
    }
}
