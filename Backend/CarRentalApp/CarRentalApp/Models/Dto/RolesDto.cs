using System.ComponentModel.DataAnnotations;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Models.Dto;

public class RolesDto
{
    [Required]
    public ICollection<Roles> Roles { get; set; } = null!;
}