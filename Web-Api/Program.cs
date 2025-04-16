using Microsoft.EntityFrameworkCore;
using Web_Api.Data;
using Web_Api.Repositories.Interfaces;
using Web_Api.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpClient();


        // ���������� �������� � ���������� - �����������
        builder.Services.AddControllers();
        // ��������� �������� ���� ������
        builder.Services.AddDbContext<TeamProfitDBContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


        // ��������� Swager
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITeamRepository, TeamRepository>();
        builder.Services.AddScoped<ICostRepository, CostRepository>();


        var app = builder.Build();


        // ��������� HTTP
        // ������������ ��������� ��������
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection(); - ��� HTTPS

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}