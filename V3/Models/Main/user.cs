using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV3.Models.main
{
    [Table("user")]
    public partial class user
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public int employee_id { get; set; }

        public employee employee { get; set; }

        [Required]
        public int employeer_id { get; set; }

        public employer employer { get; set; }

    }
}