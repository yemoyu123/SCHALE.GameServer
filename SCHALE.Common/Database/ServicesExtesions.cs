using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace SCHALE.Common.Database
{
    public static class ServicesExtesions
    {
        public static void AddMongoDBProvider(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SCHALEContext>(opt =>
            {
                opt.UseMongoDB(connectionString, "SCHALE");
            }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);
        }
    }
}
