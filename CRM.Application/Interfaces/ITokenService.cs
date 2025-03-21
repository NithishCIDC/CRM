namespace CRM.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid user_id, Guid org_id, int role);
    }
}
