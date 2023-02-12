using Hospital.DTO.User;
using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public void DeleteUser(Guid id)
        {
            _userRepository.DeleteUserById(id);
        }

        public User GetUserById(Guid id)
        {
            return _userRepository.GetUserById(id);
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = _userRepository.GetUsers().Select(x => new UserDto()
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

            return users;
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        private async Task<string> GetRole(Guid userId)
        {
            var user = _userRepository.GetUserById(userId);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
    }
}
