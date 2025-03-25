namespace CRM.Application.DTO
{
    public class RegisterDTO
    {
        public required int BranchId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
