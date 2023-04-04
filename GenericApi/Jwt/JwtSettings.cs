﻿namespace GenericApi.Jwt
{
    public class JwtSettings : IJwtSettings
        {
            public bool ValidateIssuerSigningKey { get; set; } = true;
            public string IssuerSigningKey { get; set; } = String.Empty;
            public bool ValidateIssuer { get; set; } = true;
            public string? ValidIssuer { get; set; }
            public bool ValidateAudience { get; set;}
            public string? ValidAudience { get; set;}
            public bool RequireExpirationTime  { get; set; }
            public bool ValidateLifetime { get; set; } = true;
            public int TokenValidityInMinutes { get; set; } = 60;
            public int RefreshTokenValidityInDays { get; set; } = 7;
        }
}
