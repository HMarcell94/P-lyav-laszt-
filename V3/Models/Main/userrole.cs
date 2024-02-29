using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("userroles")]
    public partial class userrole
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public ICollection<employee> employees { get; set; }

    }
}