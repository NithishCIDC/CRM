using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace CRM.domain.Model
{
    [Index(nameof(Email), IsUnique = true)]
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; } = DateTime.Now;
        public string? Created_By { get; set; }
        public string? Updated_By { get; set; }
    }
}
