using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalasztoV2.Models.main
{
    [Table("roles")]
    public partial class Role
    {
        [Key]
        [Required]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        public ICollection<Application> applications { get; set; }

        public ICollection<Employee> employees { get; set; }

        public ICollection<Supportrole> supportroles { get; set; }

    }
}