using GenericApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GenericApi.Extensions
{
    public static class AddJWTTokenServicesExt
    {
        public static void AddJWTTokenServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            // Add Jwt Setings
            var bindJwtSettings = new JwtSettings();
            Configuration.Bind("JWT", bindJwtSettings);
            Services.AddSingleton(bindJwtSettings);

            Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey!)),
                    ValidateIssuer = bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ValidateLifetime = bindJwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1),
                    //ClockSkew = TimeSpan.Zero,
                };
            });
        }
    }
}
