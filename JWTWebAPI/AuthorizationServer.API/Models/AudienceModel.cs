using System.ComponentModel.DataAnnotations;

namespace AuthorizationServer.API.Models
{
    public class AudienceModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}