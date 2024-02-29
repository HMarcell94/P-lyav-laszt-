using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("locations")]
    public partial class location
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

        public ICollection<employee> employees { get; set; }

    }
}