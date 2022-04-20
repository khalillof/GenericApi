namespace GenericApi.Jwt
{
    public interface IJwtSettings
    {
        bool ValidateIssuerSigningKey { get; }
        string IssuerSigningKey { get;}
        bool ValidateIssuer { get;}
        string? ValidIssuer { get;}
        bool ValidateAudience { get;}
        string? ValidAudience { get;}
        bool RequireExpirationTime { get;}
        bool ValidateLifetime { get;}
        int TokenValidityInMinutes { get;}
        int RefreshTokenValidityInDays { get;}
    }
}
