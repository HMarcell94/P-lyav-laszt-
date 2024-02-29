using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalasztoV2.Models.main
{
    [Table("adminroles")]
    public partial class Adminrole
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleID { get; set; }

        public ICollection<Employee> employees { get; set; }

    }
}