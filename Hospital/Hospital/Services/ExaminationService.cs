using Hospital.DTO.Examination;
using Hospital.Helpers;
using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Services
{
    public class ExaminationService : IExaminationService
    {
        private readonly IExaminationRepository _examinationRepository;
        private readonly IPatientService _patientService;
        private readonly UserManager<User> _userManager;
        public ExaminationService(IExaminationRepository examinationRepository, IPatientService patientService,
            UserManager<User> userManager)
        {
            _examinationRepository = examinationRepository;
            _patientService = patientService;
            _userManager = userManager;
        }

        public void CreateExamination(Examination examination)
        {
            _examinationRepository.CreateExamination(examination);
        }

        public void DeleteExamination(Guid examinationId)
        {
            _examinationRepository.DeleteExamination(examinationId);
        }

        public async Task<UpdateExaminationDto> GetExaminationDto(Guid examinationId, string currentUserId)
        {
            var examination = _examinationRepository.GetExamination(examinationId);
            var patient = _patientService.GetPatientById(examination.PatientId);
            var mappedPatient = _patientService.MapPatient(patient);

            var model = new UpdateExaminationDto()
            {
                Id = examinationId,
                ExaminationDate = examination.ExaminationDate,
                Name = examination.Name,
                NurseId = examination.NurseId.ToString(),
                Patient = mappedPatient
            };

            var nurses = await _userManager.GetUsersInRoleAsync(RoleType.Nurse.ToString());
            if (nurses.Any())
            {
                model.Nurses = nurses.Select(x =>
                             new SelectListItem() { Text = x.ToString() + " (Pielęgniarka)", Value = x.Id.ToString() }).ToList();
            }

            return model;
        }

        public IEnumerable<ViewExaminationDto> GetUserExamiantions(Guid userId)
        {
            var examinations = _examinationRepository.GetUserExamiantions(userId).ToList();
            foreach(var examination in examinations)
            {
                if(examination.ExaminationDate.Date < DateTime.Now.Date)
                {
                    examination.Status = Status.Finished;
                    _examinationRepository.UpdateExamination(examination);
                }
            }
            var model = examinations.Select(x => new ViewExaminationDto()
            {
                Id = x.Id,
                ExaminationDate = x.ExaminationDate,
                Name = x.Name,
                PatientFullName = x.Patient.ToString(),
                Status = Mapper.MapStatus(x.Status)
            });

            return model;
        }

        public Examination GetExamination(Guid examinationId)
        {
            return _examinationRepository.GetExamination(examinationId);
        }

        public void UpdateExamination(Examination examination)
        {
            _examinationRepository.UpdateExamination(examination);
        }
    }
}
