using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BlazorApp.Pages;
using Plk.Blazor.DragDrop;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddBlazorDragDrop();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Добавляем CORS политику
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешить любой источник
              .AllowAnyMethod()  // Разрешить любые методы (GET, POST, и т.д.)
              .AllowAnyHeader(); // Разрешить любые заголовки
    });
});

// Подключение BlazorServer к WebApi
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5091/api/");    // Указание базового адреса для запросов
});

// Перенаправим все `@inject HttpClient` на наш именованный клиент
builder.Services.AddScoped(sp =>
sp.GetRequiredService<IHttpClientFactory>()
      .CreateClient("ApiClient")
);


var app = builder.Build();

// Включаем CORS
app.UseCors("AllowAll");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();
app.UseRouting();


app.MapControllers();                          // ← Мэпим контроллеры на /api/*
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
