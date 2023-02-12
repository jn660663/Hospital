using Hospital.DTO.Examination;
using Hospital.DTO.Patient;
using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.Services.Interfaces;

namespace Hospital.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserService _userService;

        public PatientService(IPatientRepository patientRepository, IUserService userService)
        {
            _patientRepository = patientRepository;
            _userService = userService;
        }

        public void CreatePatient(CreatePatientDto request, Guid doctorId)
        {
            var patient = new Patient()
            {
                Name = request.Name,
                LastName = request.LastName,
                Age = request.Age,
                Pesel = request.Pesel,
                DoctorId = doctorId
            };

            _patientRepository.CreatePatient(patient);
        }

        public void DeletePatient(Guid patientId)
        {
            _patientRepository.DeletePatient(patientId);
        }

        public IEnumerable<Patient> GetDoctorPatients(Guid doctorId)
        {
            return _patientRepository.GetDoctorPatients(doctorId);
        }

        public Patient GetPatientById(Guid patientId)
        {
            return _patientRepository.GetPatientById(patientId);
        }

        public void UpdatePatient(Patient patient)
        {
            _patientRepository.UpdatePatient(patient);
        }

        public PatientDto MapPatient(Patient patient)
        {
            var model = new PatientDto();
            model.Id = patient.Id;
            model.Name = patient.Name;
            model.LastName = patient.LastName;
            model.Age = patient.Age;
            model.Pesel = patient.Pesel;
            model.DoctorFullName = _userService.GetUserById(patient.DoctorId).ToString();
            model.Examinatons = MapExaminations(patient.Examinations).ToList();
            return model;
        }

        private IEnumerable<ExaminationDto> MapExaminations(IEnumerable<Examination> examinations)
        {
            if (examinations != null)
            {
                return examinations.Select(x => new ExaminationDto()
                {
                    Name = x.Name,
                    ExaminationDate = x.ExaminationDate,
                    NurseFullName = _userService.GetUserById(x.NurseId).ToString()
                });
            }

            return new List<ExaminationDto>();
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _patientRepository.GetAllPatients();
        }
    }
}