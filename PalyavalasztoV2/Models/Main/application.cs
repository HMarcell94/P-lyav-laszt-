using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalasztoV2.Models.main
{
    [Table("applications")]
    public partial class Application
    {
        [Key]
        [Required]
        public int ApplicationID { get; set; }

        [Required]
        public int JobId { get; set; }

        public Job job { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public Supportrole supportrole { get; set; }

        public Role role { get; set; }

        public Employee employee { get; set; }

        public DateTime? ApplicationDate { get; set; }

    }
}