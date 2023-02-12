using Hospital.DTO.Examination;
using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface IExaminationService
    {
        public void CreateExamination(Examination examination);
        public IEnumerable<ViewExaminationDto> GetUserExamiantions(Guid userId);
        public void DeleteExamination(Guid examinationId);
        public Task<UpdateExaminationDto> GetExaminationDto(Guid examinationId, string currentUserId);
        public void UpdateExamination(Examination examination);
        public Examination GetExamination(Guid examinationId);
    }
}