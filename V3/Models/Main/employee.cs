using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("employees")]
    public partial class employee
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }

        public employer employer { get; set; }

        public userrole userrole { get; set; }

        public adminrole adminrole { get; set; }

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

        public role role { get; set; }

        public int? LocationId { get; set; }

        public location location { get; set; }

        public ICollection<application> applications { get; set; }

        public ICollection<user> users { get; set; }

    }
}