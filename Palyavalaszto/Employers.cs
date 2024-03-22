using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Module.SQL_Models
{

    [Table("Employers")]

    public class Class
    {
        [Key]
        [Required]
        public int EmployerID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public text Description { get; set; }

        [Required]
        public blob ProfilePicture { get; set; }

        [Required]
        public varbinary PasswordHash { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int LocationID { get; set; }
    }
}