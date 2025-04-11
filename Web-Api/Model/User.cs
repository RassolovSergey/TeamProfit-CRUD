using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace Web_Api.Model
{
    public class User
    {
        // Уникальный идентификатор пользователя
        [Key]
        [Required]
        public int IdUser { get; set; }

        // Никнэйм пользователя
        [Required, MaxLength(30)]
        public string Login { get; set; }

        // Имя пользователя
        [MaxLength(30)]
        public string ? Name { get; set; }

        [MaxLength(30)]
        public string ? Surname { get; set; }

        // Email пользователя (обязательное поле, должен быть валидный email, максимальная длина 255 символов)
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; }

        // Хэш пароля
        [Required]
        public string PasswordHash { get; set; }

        // Соль для хэширования пароля
        public string ? Salt { get; set; }

        // Роль пользователя
        [Required]
        public Role Role { get; set; }

        // Тип сотрудничества
        [Required]
        public TypeCooperation TypeCooperation { get; set; }

        // Цена работы
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Цена должна быть больше нуля!")]
        public decimal PriceWork { get; set; } = 10;

        // ID Команды
        public int ? IdTeam { get; set; }

        // Логическое поле активности пользователя
        [Required]
        public bool IsActive { get; set; } = false;

        // Дата регистрации
        [Required]
        public DateTime DateRegistration { get; set; } = DateTime.UtcNow;

        public Team ? Team { get; set; }  // Связь с командой (ссылка на объект Team)

        // Связь с затратами (пользователь может иметь несколько статей расходов)
        public ICollection<Cost> ? Costs { get; set; }  // Добавляем коллекцию затрат
    }


    // Типы финансового сотрудничества
    public enum TypeCooperation
    {
        Сдельная,
        Процентная
    }

    // Список ролей
    public enum Role
    {
        Admim,
        Administrator,
        Creator,
        Designer,
        Artist2D,
        Artist3D,
        PRManager,
        VideoEditor,
        Worker
    }
}
