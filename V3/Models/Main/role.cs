using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("roles")]
    public partial class role
    {
        [Key]
        [Required]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        public ICollection<application> applications { get; set; }

        public ICollection<employee> employees { get; set; }

        public ICollection<supportrole> supportroles { get; set; }

    }
}