using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.API.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        [MaxLength(32)]
        public string ClientId { get; set; }
        public virtual Audience Audience { get; set; }
    }
}