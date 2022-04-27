﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalDal.Models
{
    public class Car
    {
        public Guid Id { get; set; }

        public Guid RentalCenterId { get; set; }

        public int CarTypeId { get; set; }

        [Column(TypeName = "nchar(7)")]
        public string RegistrationNumber { get; set; } = null!;

        public RentalCenter RentalCenter { get; set; } = null!;

        public CarType CarType { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = null!;
    }
}