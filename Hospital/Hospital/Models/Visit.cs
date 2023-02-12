using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Visit
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual User Doctor { get; set; }
        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public DateTime VisitDate { get; set; }
        public string Recognition { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Finished,
        Planned
    }
}