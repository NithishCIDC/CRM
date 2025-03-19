using CRM.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace CRM.Application.Service
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _expireduration = TimeSpan.FromMinutes(5);

        public OtpService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string GenerateOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _memoryCache.Set(otp, email, _expireduration);

            return otp;
        }

        public bool VerifyOtp(string otp)
        {
            if (_memoryCache.TryGetValue(otp, out string? email))
            {
                _memoryCache.Remove(otp);
                _memoryCache.Set("verify_email", email, _expireduration);
                return true;
            }
            return false;
        }

        public string? GetVerifiedEmail()
        {
            return _memoryCache.TryGetValue("verify_email", out string? email) ? email : null;
        }
    }
}
