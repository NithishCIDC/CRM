using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTO
{
    public class AuthResponseToken
    {
        public bool Success { get; } = true;
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? Message { get; set; }
    }
    public class AuthResponseSuccess
    {
        public bool Success { get; } = true;
        public string? Message { get; set; }
    }

    public class AuthResponseError
    {
        public bool Success { get; } = false;
        public string? Error { get; set; }
    }
}
