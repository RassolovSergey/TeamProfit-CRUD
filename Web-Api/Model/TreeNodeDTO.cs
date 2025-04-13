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
        public string NodeType { get; set; } // Тип узла (например, "Cost")
        public int NodeId { get; set; } // Идентификатор узла, который обновляется
        public int NewParentId { get; set; } // Идентификатор нового родителя (например, проект для затрат)
    }
}
