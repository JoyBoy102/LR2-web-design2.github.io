using System.ComponentModel.DataAnnotations;

namespace LR2WebDesign.Controllers.APIRequestBodies
{
    public class UserLoginRequestDTO
    {
        [Required(ErrorMessage = "Login или Email обязательны для заполнения")]
        public string LoginOrEmail { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        public string Password { get; set; }

    }
}
