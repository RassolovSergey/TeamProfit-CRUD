namespace Web_Api.Model
{
    public class TreeNodeDto
    {
        public int Id { get; set; } // Идентификатор узла
        public string Name { get; set; } // Имя узла (например, логин пользователя или тип затрат)
        public string NodeType { get; set; } // Тип узла (User, Cost, Team)
        public bool HasChildren { get; set; } // Флаг наличия дочерних узлов
        public List<TreeNodeDto> Children { get; set; } = new List<TreeNodeDto>(); // Список дочерних узлов

        // Новые свойства для связи с родителем
        public int ParentId { get; set; } // Идентификатор родительского узла
        public string ParentType { get; set; } // Тип родителя (например, "Team" или "Project")
    }


    public class UpdateNodeDto
    {
        public string NodeType { get; set; } = null!;
        public int Id { get; set; }

        public string? Name { get; set; }       // для команды и пользователя
        public decimal? Amount { get; set; }    // для затрат
    }

    public class CreateNodeDto
    {
        public string NodeType { get; set; } = null!; // "Team", "User", "Cost"
        public int? ParentId { get; set; }            // родитель: Team.Id или User.Id
        public string? Name { get; set; }             // имя команды или логин
        public decimal? Amount { get; set; }          // для затрат
        public string? Email { get; set; }            // для пользователя
        public string? Password { get; set; }         // для пользователя
    }
}
