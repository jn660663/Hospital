using Hospital.DTO.Patient;
using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface IPatientService
    {
        public void CreatePatient(CreatePatientDto request, Guid doctorId);
        public IEnumerable<Patient> GetDoctorPatients(Guid doctorId);
        public Patient GetPatientById(Guid patientId);
        public void UpdatePatient(Patient patient);
        public void DeletePatient(Guid patientId);
        public PatientDto MapPatient(Patient patient);
        public IEnumerable<Patient> GetAllPatients();
    }
}