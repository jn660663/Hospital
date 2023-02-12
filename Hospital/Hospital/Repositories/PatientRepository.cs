using Hospital.Models;
using Hospital.Repositories.Interfaces;

namespace Hospital.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreatePatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public void DeletePatient(Guid patientId)
        {
            var patient = GetPatientById(patientId);
            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _context.Patients.Where(_ => true);
        }

        public IEnumerable<Patient> GetDoctorPatients(Guid doctorId)
        {
            return _context.Patients.Where(x => x.DoctorId == doctorId);
        }

        public Patient GetPatientById(Guid patientId)
        {
            return _context.Patients.Where(x => x.Id == patientId).FirstOrDefault();
        }

        public void UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }
    }
}