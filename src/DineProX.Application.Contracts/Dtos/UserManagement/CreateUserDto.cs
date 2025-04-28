using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DineProX.Dtos.UserManagement
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(32)]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(32)]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public Guid RoleId { get; set; }
    }
}
