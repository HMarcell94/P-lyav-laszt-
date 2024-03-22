using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("applications")]
    public partial class application
    {
        [Key]
        [Required]
        public int ApplicationID { get; set; }

        [Required]
        public int JobId { get; set; }

        public job job { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public supportrole supportrole { get; set; }

        public role role { get; set; }

        public employee employee { get; set; }

        public DateTime? ApplicationDate { get; set; }

    }
}