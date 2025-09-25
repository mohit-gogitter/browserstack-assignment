using LogStreamingService.Services;

namespace LogStreamingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddSingleton<LogStreamerService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseCors();
            app.MapGet("/logs/stream", LogEndPoints.StreamLogs);

            app.Run();
        }
    }
}
