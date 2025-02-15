﻿using Microsoft.AspNetCore.Http;

namespace AlvaCleanCRM.Models.DTOs
{
    public class EmployeerToUpdateDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public IFormFile Image { get; set; }

        public string ImageId { get; set; }
    }
}
