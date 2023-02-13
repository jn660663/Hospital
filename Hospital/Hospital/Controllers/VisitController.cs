using Hospital.DTO.Visit;
using Hospital.Models;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
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
        public async Task<IActionResult> GetVisits()
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = await GetRole(currentUserId);
            if(role == RoleType.Doctor.ToString())
            {
                return View(_visitService.GetDoctorVisits(currentUserId));
            }

            return View(_visitService.GetAllVisits());
            
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

        [Authorize(Roles = $"{nameof(RoleType.Doctor)}")]
        [HttpPost]
        public async Task<IActionResult> CreateVisit(CreateVisitDto request)
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));        

            var visit = new Visit()
            {
                PatientId = Guid.Parse(request.PatientId),
                VisitDate = DateTime.Now,
                Recognition = request.Recognition,
                Description = request.Description,
                DoctorId = currentUserId,
                Status = Status.Finished
            };

            _visitService.CreateVisit(visit);
            return RedirectToAction(nameof(GetVisits));
        }

        [Authorize(Roles = $"{nameof(RoleType.Receptionist)}")]
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
        [Authorize(Roles = $"{nameof(RoleType.Receptionist)}")]
        public IActionResult AssignVisitToDoctor(AssignVisitToDoctorDto request)
        {
            var visit = new Visit()
            {
                DoctorId = Guid.Parse(request.DoctorId),
                PatientId = Guid.Parse(request.PatientId),
                VisitDate = request.VisitDate,
                Status = Status.Planned
            };
            _visitService.CreateVisit(visit);
            return RedirectToAction(nameof(GetVisits));
        }

        [Authorize(Roles = $"{nameof(RoleType.Doctor)}, {nameof(RoleType.Receptionist)}")]
        public IActionResult UpdateVisit(Guid visitId)
        {
            var model = new UpdateVisitDto();
            var visit = _visitService.GetVisitById(visitId);
            var patient = _patientService.GetPatientById(visit.PatientId);
            model.Id = visit.Id;
            model.Patient = patient.Name + " " + patient.LastName + " (" + patient.Pesel + ")";
            model.Description = visit.Description;
            model.Recognition = visit.Recognition;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleType.Doctor)}, {nameof(RoleType.Receptionist)}")]
        public async Task<IActionResult> UpdateVisit(UpdateVisitDto request)
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = await GetRole(currentUserId);
            var visit = _visitService.GetVisitById(request.Id);
            if(role == RoleType.Doctor.ToString())
            {
                visit.Status = Status.Finished;
            }
            else
            {
                visit.Status = Status.Planned;
            }
            visit.Recognition = request.Recognition;
            visit.Description = request.Description;
            _visitService.UpdateVisit(visit);
            return RedirectToAction(nameof(GetVisits));
        }

        [Authorize(Roles = $"{nameof(RoleType.Doctor)}, {nameof(RoleType.Receptionist)}")]
        public IActionResult ViewVisit(Guid visitId)
        {
            var visit = _visitService.GetVisitById(visitId);
            var patient = _patientService.GetPatientById(visit.PatientId);
            var mappedPatient = _patientService.MapPatient(patient);
            var model = new ViewVisitDto()
            {
                Description = visit.Description,
                VisitDate = visit.VisitDate,
                Recognition = visit.Recognition,
                Patient = mappedPatient
            };
            return View(model);
        }

        [Authorize(Roles = $"{nameof(RoleType.Doctor)}, {nameof(RoleType.Receptionist)}")]
        public IActionResult DeleteVisit(Guid visitId)
        {
            _visitService.DeleteVisit(visitId);
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
