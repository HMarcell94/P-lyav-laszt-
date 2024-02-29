using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV4.Models.v4_1
{
    [Table("roles")]
    public partial class role
    {
        [Key]
        [Required]
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public ICollection<user> users { get; set; }

    }
}