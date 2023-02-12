using Hospital.DTO.Patient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Examination
{
    public class CreateExaminationDto
    {
        [Display(Name = "Nazwa badania")]
        public string Name { get; set; }
        public PatientDto Patient { get; set; }
        public string NurseId { get; set; }
        [Display(Name = "Osoba wykonująca badanie")]
        public List<SelectListItem> Nurses { get; set; }
        [Display(Name = "Data badania")]
        public DateTime ExaminationDate { get; set; }
    }
}