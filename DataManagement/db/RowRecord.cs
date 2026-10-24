using System.ComponentModel.DataAnnotations;

namespace DataManagement.db {
    public class RowRecord {
        [Required(ErrorMessage = "Обязательно надо заполнить тип объекта!")]
        [AllowedValues("коммерческое помещение", "жилое помещение", "дом", "участок", ErrorMessage = "Недопустимый тип объекта!")]
        public string? type { get; set; }

        [Required(ErrorMessage = "Обязательно надо заполнить площадь объекта!")]
        [Range(1, 1000000, ErrorMessage = "Недопустимая площадь")]
        public float? square { get; set; }

        [Required(ErrorMessage = "Обязательно надо заполнить цену объекта!")]
        [Range(typeof(decimal), "1", "9999999999", ErrorMessage = "Цена должна быть в диапазоне от 1 до 9 999 999 999 рублей.")]
        public decimal? price { get; set; }

        [Required(ErrorMessage = "Обязательно надо установить статус объекта.")]
        [AllowedValues("активное", "не активное", "в заморозке", ErrorMessage = "Допустимые значения - активное / не активное / в заморозке")]
        public string? status { get; set; }

        [Required(ErrorMessage = "Обязательно указывается количество комнат")]
        [Range(0, 20, ErrorMessage = "Допстимое количество комнат до 20.")]
        public int? numberOfRooms { get; set; }

        [Required(ErrorMessage = "Обязательно необходимо указать количество этажей.")]
        [Range(1, 100, ErrorMessage = "Допстимое количество этажей до 100.")]
        public int? countFloor {  get; set; }

        [Required(ErrorMessage = "Обязательно надо указать наличие лифта.")]
        public bool? elevator {  get; set; }

        [Required(ErrorMessage = "Обязательно надо заполнить поле *ремонт*")]
        [AllowedValues("без ремонта", "косметический", "евроремонт", "дизайнерский", ErrorMessage = "Допустимые значения пол: без ремонта, косметический, евроремонт, дизайнерский")]
        public string? repair { get; set; }
        [StringLength(500, ErrorMessage = "Слишком длинное описание.")]
        public string? description { get; set; }

        [Required(ErrorMessage = "Обязательно необходимо заполнить адресс объекта.")]
        [StringLength(100, ErrorMessage = "Адресс слишком длинный")]
        public string? adress { get; set; }
    }
}
