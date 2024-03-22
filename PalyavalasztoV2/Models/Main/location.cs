using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalasztoV2.Models.main
{
    [Table("locations")]
    public partial class Location
    {
        [Key]
        [Required]
        public int LocationId { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public decimal Latttude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public ICollection<Employee> employees { get; set; }

    }
}