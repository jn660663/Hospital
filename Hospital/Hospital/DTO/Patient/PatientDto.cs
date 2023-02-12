using Hospital.DTO.Examination;
using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Patient
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Imię")]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Wiek")]
        public int Age { get; set; }
        [Display(Name = "Pesel")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Dozwolone są tylko cyfry")]
        public string Pesel { get; set; }
        [Display(Name = "Lekarz prowadzący")]
        public string DoctorFullName { get; set; }
        public List<ExaminationDto> Examinatons { get; set; }
    }
}