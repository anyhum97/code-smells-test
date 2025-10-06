using System.ComponentModel.DataAnnotations;

namespace code_smells_test.Models
{
	public class Ticket
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Название
        /// </summary>
		[Required(ErrorMessage = "Название обязательно для заполнения")]
        [MaxLength(200, ErrorMessage = "Название не должно превышать 200 символов")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Описание
        /// </summary>
		[MaxLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
        public string? Description { get; set; }

        /// <summary>
        /// Дата посещения
        /// </summary>
        public DateTime VisitDate { get; set; }

        /// <summary>
        /// Количество людей
        /// </summary>
		[Range(1, 10, ErrorMessage = "Количество людей должно быть в диапазоне от 1 до 10")]
        public int VisitorsNumber { get; set; }
    }
}
