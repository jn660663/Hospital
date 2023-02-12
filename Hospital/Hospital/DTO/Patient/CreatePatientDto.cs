using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Patient
{
    public class CreatePatientDto
    {
        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string LastName { get; set; }

        [Display(Name = "Wiek")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public int Age { get; set; }

        [Display(Name = "Pesel")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Pesel musi się składać z 11 cyfr")]
        public string Pesel { get; set; }
    }
}