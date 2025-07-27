using System;

namespace DineProX.Dtos.CustomerManagement
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Guid? UserId { get; set; }
        
        // Only if needed
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
    }
} 