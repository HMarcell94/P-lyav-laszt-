using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("adminroles")]
    public partial class adminrole
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleID { get; set; }

        public ICollection<employee> employees { get; set; }

    }
}