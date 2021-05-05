using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Petz.Dal.Entities;

namespace Petz.Web.Hosting
{
    public static class HostDataExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host) where TContext : IdentityDbContext<User>
        {
            using(var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();
            }

            return host;
        }
    }
}