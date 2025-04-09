namespace CRM.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid user_id,string email, Guid org_id, Guid role, IEnumerable<string> permission);
    }
}
