using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMuser.Application.Interfaces
{
    public interface IOtpService
    {

        string GenerateOtp(string email);

        bool VerifyOtp(string otp);

        string GetVerifiedEmail();

    }
}
