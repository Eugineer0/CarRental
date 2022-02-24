﻿using System.ComponentModel.DataAnnotations;
using CarRentalApp.ValidationAttributes;

namespace CarRentalApp.Models.Dto.Registration
{
    public class ClientRegistrationDto : UserRegistrationDto
    {
        [Required]
        [RegularExpression(
            "[0-9]{1}[A-Z]{2}[0-9]{6}",
            ErrorMessage = "Incorrect format: The {0} value must consist of 1 digit leading 2 capitals, followed by 6 digits"
        )]
        public override string DriverLicenseSerialNumber { get; set; } = null!;

        [Required]
        [MinimumAge(minimumAge: 19, ErrorMessage = "Incorrect input: Client have to reach {1} years")]
        public override DateTime? DateOfBirth { get; set; }
    }
}