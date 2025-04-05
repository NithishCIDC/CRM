using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTO
{
    public class VerifyOtpDTO
    {
        public required string Email { get; set; }
        public required string Otp { get; set; }
    }
}
