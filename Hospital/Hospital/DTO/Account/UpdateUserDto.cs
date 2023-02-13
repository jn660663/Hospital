using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Account
{
    public class UpdateUserDto
    {
        [Required]
        public Guid Id { get; set; }
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
        public string Role { get; set; }
    }
}