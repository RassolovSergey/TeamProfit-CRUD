using Microsoft.EntityFrameworkCore;
using Web_Api.Data;

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