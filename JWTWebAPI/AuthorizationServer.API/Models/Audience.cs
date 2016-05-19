using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.API.Models
{
    public class Audience
    {
        [Key]
        [MaxLength(32)]
        public string ClientId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Base64Secret { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}