namespace BlazorApp.Model
{
    public class TreeNodeDto
    {
        public int Id { get; set; } // Идентификатор узла
        public string Name { get; set; } // Имя узла (например, логин пользователя или тип затрат)
        public string NodeType { get; set; } // Тип узла (User, Cost, Team, Project)
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

    public class UserDto
    {
        public int UserId { get; set; }  // Идентификатор пользователя
        public string Username { get; set; }  // Логин пользователя
        public string Email { get; set; }  // Электронная почта пользователя
    }

    public class OutfitDto
    {
        public int OutfitId { get; set; }  // Идентификатор аутфита
        public string Title { get; set; }  // Название аутфита
        public string Description { get; set; }  // Описание аутфита
        public string ImageUrl { get; set; }  // URL изображения аутфита
        public int UserId { get; set; }  // Идентификатор пользователя (родитель)
    }

    public class CommentDto
    {
        public int CommentId { get; set; }  // Идентификатор комментария
        public string Text { get; set; }  // Текст комментария
        public int OutfitId { get; set; }  // Идентификатор аутфита (родитель)
    }

    public class ModalResult
    {
        public TreeNodeDto CreatedNode { get; set; }  // Узел, который был создан
        public TreeNodeDto UpdatedNode { get; set; }  // Узел, который был обновлен
    }
}
