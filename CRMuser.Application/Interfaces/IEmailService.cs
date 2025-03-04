using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMuser.Application.Interfaces
{
    public interface IEmailService
    {
        void Email(string email, string subject, string bodyContent);
    }
}
