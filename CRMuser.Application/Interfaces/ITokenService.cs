﻿namespace CRMuser.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string email, string name);
    }
}
