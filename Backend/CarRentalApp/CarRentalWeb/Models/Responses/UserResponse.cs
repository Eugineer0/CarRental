﻿using SharedResources.EnumsAndConstants;

namespace CarRentalWeb.Models.Responses
{
    public class UserResponse
    {
        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string PassportNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string? DriverLicenseSerialNumber { get; set; }

        public IEnumerable<Role> Roles { get; set; } = null!;

        public bool ApprovalRequested { get; set; }
    }
}