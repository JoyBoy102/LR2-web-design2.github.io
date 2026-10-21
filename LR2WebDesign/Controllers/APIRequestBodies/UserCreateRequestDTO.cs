using System.ComponentModel.DataAnnotations;

namespace LR2WebDesign.Controllers.APIRequestBodies
{
    public class UserCreateRequestDTO
    {
        [Required(ErrorMessage = "Логин обязателен для заполнения")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Логин должен быть от 6 до 20 символов")]
        public string? Login { get; set; }


        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
    ErrorMessage = "Пароль должен быть не менее 6 символов, содержать хотя бы одну заглавную букву, одну строчную букву и одну цифру.")]
        public string Password { get; set; }
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        public string? Email { get; set; }

    }
}
