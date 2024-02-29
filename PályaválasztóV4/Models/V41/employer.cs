using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV4.Models.v4_1
{
    [Table("employers")]
    public partial class employer
    {
        [Key]
        [Required]
        public int EmployerID { get; set; }

        public int? UserID { get; set; }

        public user user { get; set; }

        public string CompanyName { get; set; }

        public string CompanyDescription { get; set; }

        public byte[] CompanyLogo { get; set; }

        public ICollection<job> jobs { get; set; }

    }
}