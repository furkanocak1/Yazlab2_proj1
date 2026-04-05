using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// GùVENLùK-JWT Ayarlarù
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated", p => p.RequireAuthenticatedUser());
});

// YARP Yùnlendirme Ayarlarù
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Loglama ve Trafik akùùù iùin metrikler (Hocanùn istediùi Grafana verileri)
app.UseHttpMetrics();

app.MapReverseProxy();
app.MapMetrics();

app.Run();
