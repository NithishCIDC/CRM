using System.ComponentModel.DataAnnotations;

namespace CRM.domain.Model
{
    public class MasterAdmin
    {
        [Key]
        public Guid MasterAdminId { get; set; }
        public string? MasterAdminName { get; set; }
        public string? MasterAdminPassword { get; set; }
        public string? MasterAdminEmail { get; set; }
        public int RoleId { get; set; }
    }
}
