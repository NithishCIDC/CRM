namespace CRM.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid user_id,string email, Guid org_id, int role);
        string SuperAdminGenerateToken(string email, int roleId);
    }
}
