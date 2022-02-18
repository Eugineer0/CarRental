﻿using System.ComponentModel.DataAnnotations;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Models.DTOs;

public class RolesDTO
{
    [Required]
    public ICollection<Roles> Roles { get; set; }
}