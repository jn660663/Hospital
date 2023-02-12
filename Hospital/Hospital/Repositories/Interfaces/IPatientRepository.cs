using Hospital.Models;

namespace Hospital.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public void CreatePatient(Patient patient);
        public IEnumerable<Patient> GetDoctorPatients(Guid doctorId);
        public Patient GetPatientById(Guid patientId);
        public void UpdatePatient(Patient patient);
        public void DeletePatient(Guid patientId);
        public IEnumerable<Patient> GetAllPatients();
    }
}