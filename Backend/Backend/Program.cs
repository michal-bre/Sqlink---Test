using Core.Interfaces;
using Services.Data;
using Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// הוספת השירותים למכולה (DI)
builder.Services.AddSingleton<JsonTicketRepository>(); // Singleton כי זה קובץ אחד
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<AiSummaryService>();
builder.Services.AddScoped<FileService>();

builder.Services.AddHttpClient<AiSummaryService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// הגדרת CORS עבור Angular
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // או "http://localhost:4200"
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();
app.UseStaticFiles();
app.Run();