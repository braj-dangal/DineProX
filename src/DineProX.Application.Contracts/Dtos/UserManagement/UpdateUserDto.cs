using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DineProX.Dtos.UserManagement
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        public List<Guid> RoleIds { get; set; }
    }
}
