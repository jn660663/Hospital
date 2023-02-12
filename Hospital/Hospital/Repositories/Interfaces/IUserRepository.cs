using Hospital.Models;

namespace Hospital.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public IEnumerable<User> GetUsers();
        public User GetUserById(Guid id);
        public void DeleteUserById(Guid id);
        public void UpdateUser(User user);
    }
}