using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace OpenIDWebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure your OpenID Connect client settings
            builder.Services.AddAuthentication(options =>
            {
                // sau khi authenticate thành công thì thông tin user sẽ được lưu vào Cookie
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // khi phải authenticate thì app sẽ redirect người dùng tới OpenID Connect để login
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie() // khi user login sử dụng OpenID Connect, session của họ sẽ được lưu trữ trong Cookie thay vì phải login lại cho mỗi request
            .AddOpenIdConnect(options =>
            {
                var oidcConfig = builder.Configuration.GetSection("OpenIDConnectSettings");

                options.Authority = oidcConfig["Authority"];
                options.ClientId = oidcConfig["ClientId"];
                options.ClientSecret = oidcConfig["ClientSecret"];

                // sau khi login, các thông tin authenticate sẽ được lưu vào Cookie
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // Authorization Code Flow sẽ được sử dụng, nhận về 1 authorization code sau đó gửi lên server để lấy access token
                options.ResponseType = OpenIdConnectResponseType.Code;

                // lưu các thông tin ID Token, Access Token và Refresh Token trong authentication session
                options.SaveTokens = true;
                // lấy thông tin user tại UserInfo Endpoint
                options.GetClaimsFromUserInfoEndpoint = true;

                options.MapInboundClaims = false;
                options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                options.TokenValidationParameters.RoleClaimType = "roles";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
