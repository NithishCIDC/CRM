
namespace CRMUser.domain.Interface
{
    public interface ITokenGeneration
    {
        string GenerateToken(string email,string name);
    }
}
