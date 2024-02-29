using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV4.Models.v4_1
{
    [Table("applications")]
    public partial class application
    {
        [Key]
        [Required]
        public int ApplicationID { get; set; }

        public int? JobID { get; set; }

        public job job { get; set; }

        public int? EmployeeID { get; set; }

        public employee employee { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public string ApplicationStatus { get; set; }

    }
}