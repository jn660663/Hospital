using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class Patient
    {
        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Imię")]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Wiek")]
        public int Age { get; set; }
        [Display(Name = "Pesel")]
        public string Pesel { get; set; }
        public Guid DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual User Doctor { get; set; }
        public virtual ICollection<Examination>? Examinations { get; set; }
        public override string ToString()
        {
            return Name + " " + LastName;
        }
    }
}