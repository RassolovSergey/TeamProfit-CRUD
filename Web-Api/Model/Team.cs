using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace Web_Api.Model
{
    public class Team
    {
        [Required]
        public int Id { get; set; } // ID команды

        [Required, MaxLength(20)]
        public string Name { get; set; } = string.Empty;    // Название команды

        // Связи
        public ICollection<User>? Users { get; set; } // В одной команде много пользователей
    }
}
