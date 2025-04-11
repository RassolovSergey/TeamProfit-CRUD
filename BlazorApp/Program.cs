using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BlazorApp.Pages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// ��������� CORS ��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // ��������� ����� ��������
              .AllowAnyMethod()  // ��������� ����� ������ (GET, POST, � �.�.)
              .AllowAnyHeader(); // ��������� ����� ���������
    });
});

// ����������� BlazorServer � WebApi
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5091/api/");    // �������� �������� ������ ��� ��������
});

var app = builder.Build();

// �������� CORS
app.UseCors("AllowAll");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


// app.UseHttpsRedirection();


app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
