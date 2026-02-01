using backend.Data;
using backend.Services;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Resend;
using System;

var builder = WebApplication.CreateBuilder(args);
static string ConvertPostgresUrl(string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    return $"Host={uri.Host};" +
           $"Port={uri.Port};" +
           $"Database={uri.AbsolutePath.TrimStart('/')};" +
           $"Username={userInfo[0]};" +
           $"Password={userInfo[1]};" +
           $"Ssl Mode=Require;Trust Server Certificate=true";
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<Imember,MemberService>();
builder.Services.AddOptions();
builder.Services.AddHttpClient<IResend,ResendClient>();
builder.Services.Configure<ResendClientOptions>(options =>
   { options.ApiToken = builder.Configuration["RESEND_API_KEY"]!;  });
builder.Services.AddTransient<IEmailService, EmailService>();
//builder.Services.AddDbContext<GymDbContext>(options =>
// options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<GymDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//);
//builder.Services.AddDbContext<GymDbContext>(options =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
//        ?? builder.Configuration["DATABASE_URL"];

//    options.UseNpgsql(connectionString);
//});

builder.Services.AddDbContext<GymDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? builder.Configuration["DATABASE_URL"];

    if (connectionString != null && connectionString.StartsWith("postgres"))
    {
        connectionString = ConvertPostgresUrl(connectionString);
    }

    options.UseNpgsql(connectionString);
});


builder.Services.AddCors(options=>
    options.AddPolicy("allowreact",
    policy=>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    }
)
    );
var app = builder.Build();
app.UseCors("allowreact");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
        app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GymDbContext>();
        context.Database.Migrate();
        Console.WriteLine("Database migration applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
