using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Account
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Pole e-mail jest wymagane")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole hasło jest wymagane")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}