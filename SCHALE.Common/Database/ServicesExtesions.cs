using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SCHALE.Common.Database
{
    public static class ServicesExtesions
    {
        public static void AddMongoDBProvider(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SCHALEContext>(opt =>
            {
                opt.UseMongoDB(connectionString, "SCHALE");
            });
        }
    }
}
