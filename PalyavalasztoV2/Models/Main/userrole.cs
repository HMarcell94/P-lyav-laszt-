using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalasztoV2.Models.main
{
    [Table("userroles")]
    public partial class Userrole
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public ICollection<Employee> employees { get; set; }

    }
}