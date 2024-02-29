using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PalyavalsztoV4.Models.v4_1
{
    [Table("users")]
    public partial class user
    {
        [Key]
        [Required]
        public int UserID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public byte[] ProfilePicture { get; set; }

        public int? RoleID { get; set; }

        public role role { get; set; }

        public ICollection<employee> employees { get; set; }

        public ICollection<employer> employers { get; set; }

    }
}