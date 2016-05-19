using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.API.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}