using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System;
using System.Security.Cryptography;

namespace Cloud.Jwt
{
    public static class JwtConfigureExtensions
    {
        public static void AddJwtConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var rsa = RSA.Create();
            rsa.ImportPublicKey(RSAKeyType.Pkcs8, configuration["Jwt:pubKey"]);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//validate the server
                    ValidateAudience = true,//ensure that the recipient of the token is authorized to receive it 
                    ValidateLifetime = true,//check that the token is not expired and that the signing key of the issuer is valid 
                    ValidateIssuerSigningKey = true,//verify that the key used to sign the incoming token is part of a list of trusted keys
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    ClockSkew = TimeSpan.FromSeconds(5),
                };
            });
        }
    }
}
