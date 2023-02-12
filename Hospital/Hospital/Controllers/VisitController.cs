using Hospital.DTO.Visit;
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
    public class VisitController : Controller
    {
        private readonly IVisitService _visitService;
        private readonly IPatientService _patientService;
        private readonly IExaminationService _examinationService;
        private readonly UserManager<User> _userManager;

        public VisitController(IVisitService visitService, IPatientService patientService, UserManager<User> userManager,
            IExaminationService examinationService)
        {
            _visitService = visitService;
            _patientService = patientService;
            _userManager = userManager;
            _examinationService = examinationService;
        }

        [Authorize(Roles = $"{nameof(RoleType.Doctor)}, {nameof(RoleType.Receptionist)}")]
        public IActionResult GetVisits()
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var visits = _visitService.GetDoctorVisits(currentUserId);
            return View(visits);
        }

        [Authorize(Roles = $"{nameof(RoleType.Doctor)}, {nameof(RoleType.Receptionist)}")]
        public IActionResult CreateVisit()
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = new CreateVisitDto();
            var doctorPatients = _patientService.GetDoctorPatients(currentUserId);
            model.Patients = doctorPatients.Select(x =>
                             new SelectListItem() { Text = x.ToString() + " " + x.Pesel, Value = x.Id.ToString() }).ToList();
            return View(model);
        }

        [Authorize(Roles = $"{nameof(RoleType.Doctor)}, {nameof(RoleType.Receptionist)}")]
        [HttpPost]
        public async Task<IActionResult> CreateVisit(CreateVisitDto request)
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = await GetRole(currentUserId);           

            var visit = new Visit()
            {
                PatientId = Guid.Parse(request.PatientId),
                VisitDate = DateTime.Now,
                Recognition = request.Recognition,
                Description = request.Description,
                DoctorId = currentUserId
            };

            if (role == RoleType.Doctor.ToString())
            {
                visit.Status = Status.Finished;
            }
            else
            {
                visit.Status = Status.Planned;
            }

            _visitService.CreateVisit(visit);
            return RedirectToAction(nameof(GetVisits));
        }

        public async Task<IActionResult> AssignVisitToDoctor()
        {
            var patients = _patientService.GetAllPatients();
            var doctors = await _userManager.GetUsersInRoleAsync(RoleType.Doctor.ToString());
            var model = new AssignVisitToDoctorDto()
            {
                Patients = patients.Select(x =>
                             new SelectListItem() { Text = x.ToString() + " " + x.Pesel, Value = x.Id.ToString() }).ToList(),
                Doctors = doctors.Select(x =>
                             new SelectListItem() { Text = x.ToString() + " (Lekarz)", Value = x.Id.ToString() }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AssignVisitToDoctor(AssignVisitToDoctorDto request)
        {
            var visit = new Visit()
            {
                DoctorId = Guid.Parse(request.DoctorId),
                PatientId = Guid.Parse(request.PatientId),
                VisitDate = request.VisitDate
            };
            _visitService.CreateVisit(visit);
            return RedirectToAction(nameof(GetVisits));
        }

        private async Task<string> GetRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
    }
}
