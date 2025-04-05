using CRM.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces
{
    public interface IOtpService
    {
        string GenerateOtp(string email);
        bool VerifyOtp(VerifyOtpDTO entity);
        public bool GetVerifiedEmail(string email);
    }
}
