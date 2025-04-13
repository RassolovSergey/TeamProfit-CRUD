using System.ComponentModel.DataAnnotations;

namespace Web_Api.Model
{
    public class Cost
    {
        [Required]
        public int Id { get; set; } // ID траты

        public decimal? Amounts { get; set; } = 10.0m;  // Сумма затраты

        // Связь
        public User? User { get; set; } // Ссылка на User
        public int IdUser { get; set; } // Id Пользователя
    }
}
