using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// GÜVENLÝK-JWT Ayarlarý
var key = Encoding.ASCII.GetBytes("YazlabBiletlemeSistemiCokGizliAnahtar12345!!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// YARP Yönlendirme Ayarlarý
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Loglama ve Trafik akýþý için metrikler (Hocanýn istediði Grafana verileri)
app.UseHttpMetrics();

app.MapReverseProxy();
app.MapMetrics();

app.Run();
