using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalasztoV2.Models.main
{
    [Table("employers")]
    public partial class Employer
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public byte[] Profilpicture { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int LocationId { get; set; }

        public ICollection<Employee> employees { get; set; }

        public ICollection<Job> jobs { get; set; }

    }
}