using Hospital.DTO.Visit;
using Hospital.Helpers;
using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.Services.Interfaces;

namespace Hospital.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        public VisitService(IVisitRepository visitRepository)
        {
            _visitRepository = visitRepository;
        }
        public void CreateVisit(Visit visit)
        {
            _visitRepository.CreateVisit(visit);
        }

        public void DeleteVisit(Guid visitId)
        {
            _visitRepository.DeleteVisit(visitId);
        }

        public IEnumerable<VisitDto> GetAllVisits()
        {
            var visits = _visitRepository.GetAllVisits().ToList();
            return visits.Select(x => new VisitDto()
            {
                Id = x.Id,
                Description = x.Description,
                PatientFullName = x.Patient.ToString(),
                VisitDate = x.VisitDate,
                Recognition = x.Recognition,
                Status = Mapper.MapStatus(x.Status),
                DoctorFullName = x.Doctor.ToString()
            });
        }

        public IEnumerable<VisitDto> GetDoctorVisits(Guid doctorId)
        {
            var visits = _visitRepository.GetDoctorVisits(doctorId).ToList();
            return visits.Select(x => new VisitDto()
            {
                Id = x.Id,
                Description = x.Description,
                PatientFullName = x.Patient.ToString(),
                VisitDate = x.VisitDate,
                Recognition = x.Recognition,
                Status = Mapper.MapStatus(x.Status)
            });
        }

        public Visit GetVisitById(Guid visitId)
        {
            return _visitRepository.GetVisitById(visitId);
        }

        public void UpdateVisit(Visit visit)
        {
            _visitRepository.UpdateVisit(visit);
        }
    }
}