using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV4.Models.v4_1
{
    [Table("jobs")]
    public partial class job
    {
        [Key]
        [Required]
        public int JobID { get; set; }

        public int? EmployerID { get; set; }

        public employer employer { get; set; }

        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public string JobRequirements { get; set; }

        public string JobLocation { get; set; }

        public decimal? JobSalary { get; set; }

        public ICollection<application> applications { get; set; }

    }
}