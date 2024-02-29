using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV4.Models.v4_1
{
    [Table("employees")]
    public partial class employee
    {
        [Key]
        [Required]
        public int EmployeeID { get; set; }

        public int? UserID { get; set; }

        public user user { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactNumber { get; set; }

        public byte[] Portfolio { get; set; }

        public byte[] Resume { get; set; }

        public ICollection<application> applications { get; set; }

    }
}