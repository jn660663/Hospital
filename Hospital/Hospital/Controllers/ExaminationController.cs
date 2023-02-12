using Hospital.DTO.Examination;
using Hospital.Models;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Hospital.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly IExaminationService _examinationService;
        private readonly IPatientService _patientService;
        private readonly UserManager<User> _userManager;
        public ExaminationController(IExaminationService examinationService, IPatientService patientService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _patientService = patientService;
            _examinationService = examinationService;
        }
        public IActionResult GetExaminations()
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userExaminations = _examinationService.GetUserExamiantions(currentUserId);
            return View(userExaminations);
        }

        public IActionResult DeleteExamination(Guid examinationId)
        {
            _examinationService.DeleteExamination(examinationId);
            return RedirectToAction(nameof(GetExaminations));
        }

        public async Task<IActionResult> UpdateExamination(Guid examinationId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var examination = await _examinationService.GetExaminationDto(examinationId, currentUserId);
            return View(examination);
        }

        [HttpPost]
        public IActionResult UpdateExamination(UpdateExaminationDto request)
        {
            var examination = _examinationService.GetExamination(request.Id);
            examination.Name = request.Name;
            examination.NurseId = Guid.Parse(request.NurseId);
            _examinationService.UpdateExamination(examination);
            return RedirectToAction(nameof(GetExaminations));
        }

        public async Task<IActionResult> CreateExamination(Guid patientId)
        {
            var patient = _patientService.GetPatientById(patientId);
            var mappedPatient = _patientService.MapPatient(patient);
            var model = new CreateExaminationDto();
            model.Patient = mappedPatient;
            var nurses = await _userManager.GetUsersInRoleAsync(RoleType.Nurse.ToString());
            if (nurses.Any())
            {
                model.Nurses = nurses.Select(x =>
                             new SelectListItem() { Text = x.ToString() + " (Pielęgniarka)", Value = x.Id.ToString() }).ToList();
            }
            else
            {
                var currentDoctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentDoctor = await _userManager.FindByIdAsync(currentDoctorId);
                model.Nurses = new List<SelectListItem>() { new SelectListItem() { Text = currentDoctor.ToString() + " (Lekarz)", Value = currentDoctorId } };
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateExamination(CreateExaminationDto request)
        {
            var examination = new Examination();
            examination.ExaminationDate = request.ExaminationDate;
            examination.Name = request.Name;
            examination.PatientId = request.Patient.Id;
            examination.NurseId = Guid.Parse(request.NurseId);
            _examinationService.CreateExamination(examination);
            return RedirectToAction(nameof(GetExaminations));
        }
    }
}