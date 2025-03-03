using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMUser.domain.Interface
{
    public interface ITokenGeneration
    {
        string GenerateToken(string email);
    }
}
