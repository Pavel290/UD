using System.ComponentModel.DataAnnotations;

namespace DataManagement.db {
    public class LeadRecord {
        [Required(ErrorMessage = "Необходим ввести имя клиента")]
        [StringLength(100, ErrorMessage = "Имя слишком длинное")]
        [RegularExpression(@"^[\p{L}\s\-]+$", ErrorMessage = "Имя не должно содержать цифры")]
        public string? name { get; set; }

        [Required(ErrorMessage = "Необходим ввести номер клиента")]
        [StringLength(11, ErrorMessage = "Не корректно введен номер телефона")]
        [RegularExpression(@"^(\+7|7|8)?[\s\-]?\(?[0-9]{3}\)?[\s\-]?[0-9]{3}[\s\-]?[0-9]{2}[\s\-]?[0-9]{2}$", ErrorMessage = "Номер телефона введен не корректно")]
        public string? number_phone { get; set; }

        [StringLength(500, ErrorMessage = "Слишком длинное описание.")]
        public string? description { get; set; }

        [Required(ErrorMessage = "Обязательно надо установить статус клиента.")]
        [AllowedValues("в работе", "заморожен", "не активен", ErrorMessage = "Допустимые значения - в работе / не активен / заморожен")]
        public string? status { get; set; }

        [StringLength(500, ErrorMessage = "Слишком длинное описание.")]
        public string? city { get; set; }
    }
}
