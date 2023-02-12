using Hospital.DTO.Visit;
using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface IVisitService
    {
        public Visit GetVisitById(Guid visitId);
        public void DeleteVisit(Guid visitId);
        public void CreateVisit(Visit visit);
        public IEnumerable<VisitDto> GetDoctorVisits(Guid doctorId);
        public IEnumerable<VisitDto> GetAllVisits();
        public void UpdateVisit(Visit visit);
    }
}