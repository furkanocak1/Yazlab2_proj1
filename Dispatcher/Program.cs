using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//  GÜVENLÝK-JWT Ayarlarý(AuthServicesde belirlediðimiz anahtar var)
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
            ValidateLifetime = true // Token süresi kontrolü
        };
    });

builder.Services.AddAuthorization();

//  YARP Yönlendirme Ayarlarý
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Ýsteklerin geçiþ sýrasý
app.UseRouting();

//  kimlik kontrolü 
app.UseAuthentication();
app.UseAuthorization();

//Servise yönlendiren kýsým
app.MapReverseProxy();

app.Run();