using Hospital.Models;

namespace Hospital.Repositories.Interfaces
{
    public interface IExaminationRepository
    {
        public void CreateExamination(Examination examination);
        public IEnumerable<Examination> GetUserExamiantions(Guid userId);
        public void DeleteExamination(Guid examinationId);
        public Examination GetExamination(Guid examinationId);
        public void UpdateExamination(Examination examination);
    }
}