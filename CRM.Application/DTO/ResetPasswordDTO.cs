using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTO
{
    public class ResetPasswordDTO
    {
        public required string email { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
