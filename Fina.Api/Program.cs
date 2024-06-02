using Fina.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string connectionString =
    "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;";

builder.Services.AddDbContext<AppDbContext>(
    x=>x.UseSqlServer(connectionString)
    );

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
