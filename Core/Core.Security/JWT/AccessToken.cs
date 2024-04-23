﻿namespace MenCore.Security.JWT;

public class AccessToken
{
    public AccessToken()
    {
        Token = string.Empty;
    }

    public AccessToken(string token, DateTime expiration)
    {
        Token = token;
        Expiration = expiration;
    }

    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}