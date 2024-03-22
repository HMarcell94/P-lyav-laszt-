using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalasztoV2.Models.main
{
    [Table("supportroles")]
    public partial class Supportrole
    {
        [Key]
        [Required]
        public int UserID { get; set; }

        [Required]
        public int RoleID { get; set; }

        public Role role { get; set; }

        public ICollection<Application> applications { get; set; }

    }
}