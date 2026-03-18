using AuthService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// API Uç noktalarýný (Controller'larý) sisteme dahil ediyoruz
builder.Services.AddControllers();

// API'yi tarayýcý üzerinden kolayca test etmemizi saðlayan Swagger arayüzü ayarlarý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Sisteme "IUserRepository"  gördüðünde "UserRepository" sýnýfýný çalýþtýrmasýný söylüyoruz.
builder.Services.AddScoped<IUserRepository, UserRepository>();
// -------------------------------------------------------------------

var app = builder.Build();

// (Pipeline)
if (app.Environment.IsDevelopment())
{
    // Geliþtirme aþamasýndayken Swagger arayüzünü aktif et
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();