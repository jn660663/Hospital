using Hospital.Models;
using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Examination
{
    public class ViewExaminationDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Nazwa badania")]
        public string Name { get; set; }
        public DateTime ExaminationDate { get; set; }
        public string PatientFullName { get; set; }
        public string Status { get; set; }
    }
}