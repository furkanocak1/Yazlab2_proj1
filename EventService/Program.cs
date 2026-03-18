using EventService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. API Uç noktalarýný sisteme tanýtýyoruz
builder.Services.AddControllers();
builder.Services.AddScoped<IEventRepository, EventRepository>();

// 2. Swagger (Test Arayüzü) ayarlarý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. ÝLERÝSÝ ÝÇÝN HAZIRLIK: MongoDB ve Repository Ayarlarý
// Not: IEventRepository ve EventRepository sýnýflarýný daha sonra oluþturacaðýz.
// Þimdilik yorum satýrý yapýyorum ki hata vermesin. Sýnýflarý yazýnca baþýndaki "//" iþaretlerini kaldýracaðýz.
// builder.Services.AddScoped<IEventRepository, EventRepository>();

var app = builder.Build();

// 4. Geliþtirme ortamýndaysak Swagger test ekranýný aç
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();