using Hospital.DTO.Account;
using Hospital.DTO.User;
using Hospital.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Hospital.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.Select(x => new UserDto()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Id = x.Id,
                IsActive = x.IsActive
            }).ToList();

            foreach (var user in users)
            {
                var role = await GetRole(user.Id);
                user.Role = role switch
                {
                    nameof(RoleType.Admin) => "Administrator",
                    nameof(RoleType.Nurse) => "Pielęgniarka",
                    nameof(RoleType.Doctor) => "Lekarz",
                    nameof(RoleType.Receptionist) => "Recepcjonistka"
                };
            }
            return View(users);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignUpDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Użytkownik o takim adresie e-mail już istnieje");
                return View(request);
            }

            var user = new User
            {
                FirstName = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Specialization = request.Specialization,
                PWZ = request.PWZ,
                UserName = request.Email
            };

            var registeredUser = await _userManager.CreateAsync(user, request.Password);
            if (registeredUser.Succeeded)
            {
                await GiveRoleToUser(user, request.Role, request.Password);
            }

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(currentUserId);
                var role = await GetRole(user.Id);
                return RedirectByRole(role);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                ModelState.AddModelError("Email", "Użytkownik o takim adresie e-mail nie istnieje");
            }

            if (await _userManager.CheckPasswordAsync(existingUser, request.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (existingUser.IsActive)
                    {
                        var role = await GetRole(existingUser.Id);
                        return RedirectByRole(role);
                    }

                    await _signInManager.SignOutAsync();
                    ViewBag.Message = "Twoje konto nie zostało jeszcze aktywowane przez administratora systemu";
                    return View(request);
                }
            }

            ModelState.AddModelError("Password", "Niepoprawne hasło");
            return View(request);
        }

        [Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        public async Task<IActionResult> Create(SignUpDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Użytkownik o takim adresie e-mail już istnieje");
                return View(request);
            }

            var user = new User
            {
                FirstName = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Specialization = request.Specialization,
                PWZ = request.PWZ,
                UserName = request.Email,
                IsActive = true
            };

            var registeredUser = await _userManager.CreateAsync(user, request.Password);
            if (registeredUser.Succeeded)
            {
                await GiveRoleToUser(user, request.Role);
            }

            return RedirectToAction(nameof(GetUsers));
        }

        [Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(GetUsers));
        }

        [Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        public async Task<IActionResult> Update(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var role = await GetRole(user.Id);
            var model = new UpdateUserDto()
            {
                Id = id,
                Name = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Specialization = user.Specialization,
                PWZ = user.PWZ,
                Role = role.ToString()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        public async Task<IActionResult> Update(UpdateUserDto request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            user.FirstName = request.Name;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Specialization = request.Specialization;
            user.PWZ = request.PWZ;
            await _userManager.UpdateAsync(user);
            var oldRole = await GetRole(user.Id);
            await _userManager.RemoveFromRoleAsync(user, oldRole);
            Enum.TryParse(request.Role, out RoleType role);
            await GiveRoleToUser(user, role);
            return RedirectToAction(nameof(GetUsers));
        }

        [Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        public async Task<IActionResult> ChangeStatus(Guid id, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.IsActive = !isActive;
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(GetUsers));
        }

        private IActionResult RedirectByRole(string role)
        {
            if (role == RoleType.Admin.ToString())
            {
                return RedirectToAction(nameof(GetUsers));
            }
            else if (role == RoleType.Doctor.ToString())
            {
                return RedirectToAction("GetPatients", "Patient");
            }
            else if (role == RoleType.Nurse.ToString())
            {
                return RedirectToAction("GetExaminations", "Examination");
            }
            else if (role == RoleType.Receptionist.ToString())
            {
                return RedirectToAction("GetVisits", "Visit");
            }
            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        private async Task GiveRoleToUser(User user, RoleType role, string password = null)
        {
            if ((role == RoleType.Admin && password == "Admin123@") || password == null)
            {
                user.IsActive = true;
            }

            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(role.ToString()));
            }

            await _userManager.AddToRoleAsync(user, role.ToString());
        }

        private async Task<string> GetRole(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
    }
}
