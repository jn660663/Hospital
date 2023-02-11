﻿namespace Hospital.DTO.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}