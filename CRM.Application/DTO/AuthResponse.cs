using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;

namespace CRM.Application.DTO
{
    public class AuthResponseToken
    {
        public bool Success { get; } = true;
        public string? Token { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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
