using Hospital.DTO.Patient;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IActionResult GetPatients()
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var patients = _patientService.GetDoctorPatients(currentUserId);
            return View(patients);
        }

        public IActionResult CreatePatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePatient(CreatePatientDto request)
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _patientService.CreatePatient(request, currentUserId);
            return RedirectToAction(nameof(GetPatients));
        }

        public IActionResult UpdatePatient(Guid patientId)
        {
            var patient = _patientService.GetPatientById(patientId);
            var model = new UpdatePatientDto()
            {
                Id = patientId,
                Age = patient.Age,
                Name = patient.Name,
                LastName = patient.LastName,
                Pesel = patient.Pesel
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdatePatient(UpdatePatientDto request)
        {
            var patient = _patientService.GetPatientById(request.Id);
            patient.Age = request.Age;
            patient.Name = request.Name;
            patient.LastName = request.LastName;
            patient.Pesel = request.Pesel;
            _patientService.UpdatePatient(patient);
            return RedirectToAction(nameof(GetPatients));

        }

        public IActionResult DeletePatient(Guid patientId)
        {
            _patientService.DeletePatient(patientId);
            return RedirectToAction(nameof(GetPatients));
        }

        public IActionResult ViewPatient(Guid patientId)
        {
            var patient = _patientService.GetPatientById(patientId);
            var model = _patientService.MapPatient(patient);
            return View(model);
        }
    }
}
