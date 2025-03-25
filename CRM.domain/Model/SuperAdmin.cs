using System.ComponentModel.DataAnnotations;

namespace CRM.domain.Model
{
    public class SuperAdmin
    {
        [Key]
        public Guid SuperAdminId { get; set; }
        public string? SuperAdminName { get; set; }
        public string? SuperAdminPassword { get; set; }
        public string? SuperAdminEmail { get; set; }
        public int RoleId { get; set; }
    }
}
