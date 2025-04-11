using System.ComponentModel.DataAnnotations;

namespace Web_Api.Model
{
    public class Team
    {
        [Key]
        [Required]
        public int IdTeam { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }

        // Связь с пользователями (одна команда может содержать несколько пользователей)
        public ICollection<User> ? Users { get; set; }  // Добавляем коллекцию пользователей
    }
}
