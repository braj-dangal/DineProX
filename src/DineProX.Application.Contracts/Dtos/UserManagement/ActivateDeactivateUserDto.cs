using System;
using System.Collections.Generic;
using System.Text;

namespace DineProX.Dtos.UserManagement
{
    public class ActivateDeactivateUserDto
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
