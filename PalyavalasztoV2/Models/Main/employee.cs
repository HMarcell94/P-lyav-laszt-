using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalasztoV2.Models.main
{
    [Table("employees")]
    public partial class Employee
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }

        public Employer employer { get; set; }

        public Userrole userrole { get; set; }

        public Adminrole adminrole { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Skills { get; set; }

        [Required]
        public byte[] Profilepicture { get; set; }

        [Required]
        public byte[] video { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public int RoleId { get; set; }

        public Role role { get; set; }

        public int? LocationId { get; set; }

        public Location location { get; set; }

        public ICollection<Application> applications { get; set; }

    }
}