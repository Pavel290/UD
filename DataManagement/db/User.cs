using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataManagement.db {
    public class User {
        [Required(ErrorMessage = "Необходимо указать логин для регистрации")]
        [StringLength(19, ErrorMessage = "Логин слишком длинный")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_-]{7,19}$", ErrorMessage = "Логин должен содержать от 7 до 20 символов (латиница, цифры, _ и -) и начинаться с буквы")]
        public string? login { get; set; }

        [Required(ErrorMessage = "Необходимо указать пароль для регистрации")]
        [StringLength(19, ErrorMessage = "Пароль слишком длинный")]
        [RegularExpression(@"^^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Пароль слишком слабый (нужно от 8 символов, заглавные и строчные буквы, цифры и спецсимволы)")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Необходимо указать роль для регистрации")]
        [AllowedValues("Аналитик", "Агент по  недвижиимости", "Директор", "Маркетолог", ErrorMessage = "Недопустимая роль! Допустимые значения: ")]
        public string? roly { get; set; }

        [Required(ErrorMessage = "Обязательно надо установить статус пользователя.")]
        [AllowedValues("работает", "не работает", ErrorMessage = "Допустимые значения - работает / не работает")]
        public string? status { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пользователя клиента")]
        [StringLength(100, ErrorMessage = "Имя слишком длинное")]
        [RegularExpression(@"^[\p{L}\s\-]+$", ErrorMessage = "Имя не должно содержать цифры")]
        public string? name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести номер клиента")]
        [StringLength(11, ErrorMessage = "Не корректно введен номер телефона")]
        [RegularExpression(@"^(\+7|7|8)?[\s\-]?\(?[0-9]{3}\)?[\s\-]?[0-9]{3}[\s\-]?[0-9]{2}[\s\-]?[0-9]{2}$", ErrorMessage = "Номер телефона введен не корректно")]
        public string? number_phone { get; set; }

        [StringLength(100, ErrorMessage = "Слишком длинное описание.")]
        public string? city { get; set; }

        public string hesh { get {
                var passwordHasher = new PasswordHasher();
                return passwordHasher.HashPassword(this.password);
            }
        }
    }
}
