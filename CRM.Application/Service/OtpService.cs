using CRM.Application.DTO;
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

        public bool VerifyOtp(VerifyOtpDTO entity)
        {
            if (_memoryCache.TryGetValue(entity.Otp, out string? email) && entity.Email == email)
            {
                _memoryCache.Remove(entity.Otp);
                _memoryCache.Set($"verified-Email-{email}",email,_expireduration);
                return true;
            }
            return false;
        }

        public bool GetVerifiedEmail(string email)
        {
            return _memoryCache.TryGetValue($"verified-Email-{email}",out _);
        }
    }
}
