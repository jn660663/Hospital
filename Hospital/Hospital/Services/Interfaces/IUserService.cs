using Hospital.DTO.User;
using Hospital.Models;

namespace Hospital.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDto>> GetUsers();
        public void DeleteUser(Guid id);
        public void UpdateUser(User user);
        public User GetUserById(Guid id);
    }
}