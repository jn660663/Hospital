using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Visit
{
    public class AssignVisitToDoctorDto
    {
        [Display(Name = "Pacjent")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public List<SelectListItem> Patients { get; set; }

        [Display(Name = "Lekarz")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public List<SelectListItem> Doctors { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane")]
        [Display(Name = "Data wizyty")]
        public DateTime VisitDate { get; set; }
    }
}