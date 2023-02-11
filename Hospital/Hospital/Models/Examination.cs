using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class Examination
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public Guid NurseId { get; set; }
        public DateTime ExaminationDate { get; set; }
        public ExaminationStatus Status { get; set; }
    }

    public enum ExaminationStatus
    {
        Finished,
        Planned
    }
}