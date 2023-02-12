using Hospital.Models;
using Hospital.Repositories.Interfaces;

namespace Hospital.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly ApplicationDbContext _context;
        public VisitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteVisit(Guid visitId)
        {
            var visit = _context.Visits.Where(x => x.Id == visitId).FirstOrDefault();
            _context.Visits.Remove(visit);
            _context.SaveChanges();
        }

        public void CreateVisit(Visit visit)
        {
            _context.Visits.Add(visit);
            _context.SaveChanges();
        }

        public Visit GetVisitById(Guid visitId)
        {
            return _context.Visits.Where(x => x.Id == visitId).FirstOrDefault();
        }

        public IEnumerable<Visit> GetDoctorVisits(Guid doctorId)
        {
            return _context.Visits.Where(x => x.DoctorId == doctorId);
        }

        public IEnumerable<Visit> GetAllVisits()
        {
            return _context.Visits.Where(_ => true);
        }

        public void UpdateVisit(Visit visit)
        {
            _context.Visits.Update(visit);
            _context.SaveChanges();
        }
    }
}
