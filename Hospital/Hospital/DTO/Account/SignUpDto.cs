using Hospital.Models;
using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Account
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Specjalizacja")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Numer PWZ")]
        public int? PWZ { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Rola")]
        public RoleType Role { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-.]).{8,}$", ErrorMessage = "Hasło musi zawierać cyfrę, dużą literę, znak specjalny i składać się co najmniej z 8 znaków")]
        public string Password { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Wpisane hasła nie są zgodne")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}