using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs;

public class AdminAssignmentDTO
{
    [Required]
    public string Username { get; set; }
}