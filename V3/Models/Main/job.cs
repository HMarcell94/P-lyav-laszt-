using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("jobs")]
    public partial class job
    {
        [Key]
        [Required]
        public int JobID { get; set; }

        [Required]
        public int EmployerId { get; set; }

        public employer employer { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int LocationId { get; set; }

        public ICollection<application> applications { get; set; }

    }
}