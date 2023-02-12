using Hospital.Models;

namespace Hospital.Repositories.Interfaces
{
    public interface IVisitRepository
    {
        public Visit GetVisitById(Guid visitId);
        public void DeleteVisit(Guid visitId);
        public void CreateVisit(Visit visit);
        public IEnumerable<Visit> GetDoctorVisits(Guid doctorId);
        public IEnumerable<Visit> GetAllVisits();
        public void UpdateVisit(Visit visit);
    }
}