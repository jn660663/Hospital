using Hospital.Models;

namespace Hospital.DTO.Visit
{
    public class VisitDto
    {
        public Guid Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string Recognition { get; set; }
        public string Description { get; set; }
        public string PatientFullName { get; set; }
        public string DoctorFullName { get; set; }
        public string Status { get; set; }
    }
}