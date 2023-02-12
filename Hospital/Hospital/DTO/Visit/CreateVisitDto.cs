using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Visit
{
    public class CreateVisitDto
    {
        [Display(Name = "Pacjent")]
        public List<SelectListItem> Patients { get; set; }
        public string PatientId { get; set; }

        [Display(Name = "Rozpoznanie")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string Recognition { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string Description { get; set; }
    }
}