using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("supportroles")]
    public partial class supportrole
    {
        [Key]
        [Required]
        public int userID { get; set; }

        [Required]
        public int RoleID { get; set; }

        public role role { get; set; }

        public ICollection<application> applications { get; set; }

    }
}