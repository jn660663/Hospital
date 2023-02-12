using Hospital.Models;
using Hospital.Repositories.Interfaces;

namespace Hospital.Repositories
{
    public class ExaminationRepository : IExaminationRepository
    {
        public ApplicationDbContext _context;
        public ExaminationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateExamination(Examination examination)
        {
            _context.Examinations.Add(examination);
            _context.SaveChanges();
        }

        public void DeleteExamination(Guid examinationId)
        {
            var examination = _context.Examinations.FirstOrDefault(x => x.Id == examinationId);
            _context.Examinations.Remove(examination);
            _context.SaveChanges();
        }

        public Examination GetExamination(Guid examinationId)
        {
            return _context.Examinations.FirstOrDefault(x => x.Id == examinationId);
        }

        public IEnumerable<Examination> GetUserExamiantions(Guid userId)
        {
            return _context.Examinations.Where(x => x.NurseId == userId);
        }

        public void UpdateExamination(Examination examination)
        {
            _context.Examinations.Update(examination);
            _context.SaveChanges();
        }
    }
}