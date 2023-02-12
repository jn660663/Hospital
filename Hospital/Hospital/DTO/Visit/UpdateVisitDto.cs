using System.ComponentModel.DataAnnotations;

namespace Hospital.DTO.Visit
{
    public class UpdateVisitDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Pacjent")]
        public string Patient { get; set; }

        [Display(Name = "Rozpoznanie")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string Recognition { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string Description { get; set; }
    }
}