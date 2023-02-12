using Hospital.DTO.Patient;
using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Visit
{
    public class ViewVisitDto
    {
        [Display(Name = "Data wizyty")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public DateTime VisitDate { get; set; }

        [Display(Name = "Rozpoznanie")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string Recognition { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string Description { get; set; }
        public PatientDto Patient { get; set; }
    }
}
