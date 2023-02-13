using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Examination
{
    public class ExaminationDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Nazwa badania")]
        public string Name { get; set; }
        [Display(Name = "Badanie przeprowadza")]
        public string NurseFullName { get; set; }
        [Display(Name = "Data badania")]
        public DateTime ExaminationDate { get; set; }
    }
}