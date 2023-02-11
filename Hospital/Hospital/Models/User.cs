using Microsoft.AspNetCore.Identity;

namespace Hospital.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public int PWZ { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }

    public enum RoleType
    {
        Admin,
        Doctor,
        Nurse,
        Receptionist
    }
}
