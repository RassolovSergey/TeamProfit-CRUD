using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace Web_Api.Model
{
    public class User
    {
        [Required]
        public int Id { get; set; } // ID Пользователя

        [Required, MaxLength(30)]
        public string Login { get; set; } = string.Empty;   // Login Пользователя

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = string.Empty; // Почта пользователя

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Хеш пороля

        public string ? Salt { get; set; } = string.Empty; // Соль пороля

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Цена должна быть больше нуля!")]
        public decimal PriceWork { get; set; } = 10.0m; // Цена работы

        [Required]
        public bool IsActive { get; set; } = false; // Флаг Активности

        [Required]
        public DateTime DateRegistration { get; set; } = DateTime.UtcNow; // Дата регистрации

        // Связи
        public int IdTeam { get; set; } // Serial от Team

        public Team? Team { get; set; } // Ссылка на Team

        public ICollection<Cost>? Costs { get; set; }    // У одного пользователя много трат
    }
}
