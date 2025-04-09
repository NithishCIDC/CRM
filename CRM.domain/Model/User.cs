using CRM.domain.Enum;

namespace CRM.domain.Model
{
    public class User:BaseModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public Guid RoleId { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }
    }
}
