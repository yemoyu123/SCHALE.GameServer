using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog.Events;
using Serilog;
using System.Reflection;
using SCHALE.Common.Database;
using SCHALE.GameServer.Controllers.Api.ProtocolHandlers;
using SCHALE.GameServer.Services;
using Microsoft.EntityFrameworkCore;
using SCHALE.GameServer.Services.Irc;
using SCHALE.GameServer.Commands;
using SCHALE.GameServer.Utils;
using System.Net.NetworkInformation;

namespace SCHALE.GameServer
{
    public class GameServer
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)!)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Local.json", true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            {
                var logFilePath = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory)!, "logs", "log.txt");

                if (File.Exists(logFilePath))
                {
                    var prevLogFilePath = Path.Combine(Path.GetDirectoryName(logFilePath)!, "log-prev.txt");
                    if (File.Exists(prevLogFilePath))
                        File.Delete(prevLogFilePath);

                    File.Move(logFilePath, prevLogFilePath);
                }

                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, restrictedToMinimumLevel: LogEventLevel.Verbose, shared: true)
                .ReadFrom.Configuration(config)
                .CreateBootstrapLogger();
            }

            Log.Information("Starting...");
            try
            {
                // Load Commands
                CommandFactory.LoadCommands();

                // Load Config
                Config.Load();

                if (Config.Instance.Address == "127.0.0.1")
                {
                    Config.Instance.Address = NetworkInterface.GetAllNetworkInterfaces().Where(i => i.NetworkInterfaceType != NetworkInterfaceType.Loopback && i.OperationalStatus == OperationalStatus.Up).First().GetIPProperties().UnicastAddresses.Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().Address.ToString();
                    Config.Save();
                }

                var builder = WebApplication.CreateBuilder(args);

                builder.Services.Configure<KestrelServerOptions>(op => op.AllowSynchronousIO = true);
                builder.Host.UseSerilog();

                // Add services to the container.
                builder.Services.AddSQLServerProvider(config.GetConnectionString("SQLServer") ?? throw new ArgumentNullException("ConnectionStrings/SQLServer in appsettings is missing"));
                builder.Services.AddControllers();
                builder.Services.AddProtocolHandlerFactory();
                builder.Services.AddMemorySessionKeyService();
                builder.Services.AddExcelTableService();
                builder.Services.AddIrcService();

                // Add all Handler Groups
                var handlerGroups = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(ProtocolHandlerBase)));
                
                foreach (var handlerGroup in handlerGroups)
                    builder.Services.AddProtocolHandlerGroupByType(handlerGroup);

                var app = builder.Build();

                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<SCHALEContext>();
                    if (context.Database.GetPendingMigrations().Any())
                        context.Database.Migrate();
                }

                // Configure the HTTP request pipeline.
                app.UseAuthorization();
                app.UseSerilogRequestLogging();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred during runtime");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
