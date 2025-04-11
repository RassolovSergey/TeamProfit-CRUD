using System.ComponentModel.DataAnnotations;

namespace Web_Api.Model
{
    public class Cost
    {
        [Key]  // Указание, что это первичный ключ
        [Required]
        public int IdCosts { get; set; }

        [Required]
        public int IdProject { get; set; }

        [Required]
        public TypeCosts TypeCosts { get; set; }

        [Required]
        public int IdUser { get; set; }     // Внешний ключ для связи с User

        public decimal? Amounts { get; set; } = 10;
        public decimal? Percent { get; set; } = 10;

        public User ? User { get; set; }  // Связь с пользователем (ссылка на объект User)
    }

    public enum TypeCosts
    {
        ОплатаТруда,
        ЗакупкаМатериалов,
        Реклама
    }
}
