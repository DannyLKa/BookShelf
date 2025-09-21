using BookShelf.Api.Data;
using BookShelf.Api.Repositories;
using BookShelf.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookShelf.Api", Version = "v1" });
});

// Переключатель БД
var usePg = builder.Configuration.GetValue<bool>("UsePostgres");
if (usePg)
{
    builder.Services.AddDbContext<LibraryContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("RemotePg")));
}
else
{
    builder.Services.AddDbContext<LibraryContext>(opt =>
        opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));
}

// DI
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<BookService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookShelf.Api v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
