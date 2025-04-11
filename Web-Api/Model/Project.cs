using System.ComponentModel.DataAnnotations;

namespace Web_Api.Model
{
    public class Project
    {
        // Поля Таблицы
        // Id проекта
        [Key]
        [Required]
        public int IdProject { get; set; }


        // Имя
        [Required]
        public string Name { get; set; }


        // Id команды, работающей над проектом
        public int? IdTeam { get; set; }


        // Описание проекта
        public long?  Description { get; set; }


        // Дата создания
        // Обязательный
        [Required]
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;


        // Дата завершения
        public DateTime? DateClosure { get; set; }


        // Статус - (Выбор из заготовленных)
        [Required]
        public Status Status { get; set; } = Status.Неактивен;

        public Team ? Team { get; set; }  // Связь с командой (ссылка на объект Team)
    }

    // Возможные варианты Стутуса проекта
    public enum Status
    {
        Неактивен,
        Активен,
        Завершен,
        Приостановлен,
        Отменён
    }
}
