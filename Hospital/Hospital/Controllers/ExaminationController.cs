using Hospital.DTO.Examination;
using Hospital.Models;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
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
        [Authorize(Roles = $"{nameof(RoleType.Nurse)}")]
        public IActionResult GetExaminations()
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userExaminations = _examinationService.GetUserExamiantions(currentUserId);
            return View(userExaminations);
        }

        [Authorize(Roles = $"{nameof(RoleType.Nurse)}")]
        public IActionResult DeleteExamination(Guid examinationId)
        {
            _examinationService.DeleteExamination(examinationId);
            return RedirectToAction(nameof(GetExaminations));
        }

        [Authorize(Roles = $"{nameof(RoleType.Nurse)}")]
        public async Task<IActionResult> ViewExamination(Guid examinationId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var examination = await _examinationService.GetExaminationDto(examinationId, currentUserId);
            return View(examination);
        }

        [Authorize(Roles = $"{nameof(RoleType.Nurse)}")]
        public async Task<IActionResult> UpdateExamination(Guid examinationId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var examination = await _examinationService.GetExaminationDto(examinationId, currentUserId);
            return View(examination);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleType.Nurse)}")]
        public IActionResult UpdateExamination(UpdateExaminationDto request)
        {
            var examination = _examinationService.GetExamination(request.Id);
            if(examination.Status == Status.Finished)
            {
                ViewBag.Message = "Nie można edytować zakończonych badań!";
                return RedirectToAction(nameof(GetExaminations));
            }
            examination.Name = request.Name;
            examination.NurseId = Guid.Parse(request.NurseId);
            _examinationService.UpdateExamination(examination);
            return RedirectToAction(nameof(GetExaminations));
        }

        [Authorize(Roles = $"{nameof(RoleType.Doctor)}")]
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
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleType.Doctor)}")]
        public IActionResult CreateExamination(CreateExaminationDto request)
        {
            var examination = new Examination();
            examination.ExaminationDate = request.ExaminationDate;
            examination.Name = request.Name;
            examination.PatientId = request.Patient.Id;
            examination.NurseId = Guid.Parse(request.NurseId);
            examination.Status = Status.Planned;
            _examinationService.CreateExamination(examination);
            return RedirectToAction(nameof(GetExaminations));
        }
    }
}