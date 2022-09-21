using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OhMyWhut.Engine;
using OhMyWhut.Win.Data;
using OhMyWhut.Win.Services;

namespace OhMyWhut.Win.Extentions
{
    internal static class ServiceCollectionExtention
    {
        internal static IServiceCollection AddAppDbContext(this IServiceCollection services)
        {
            return services.AddDbContext<AppDbContext>(x =>
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                x.UseSqlite($"Data Source = {System.IO.Path.Join(path, "ohmywhut.db")}");
            });
        }

        internal static IServiceCollection AddAppStatus(this IServiceCollection services)
        {
            return services.AddSingleton<AppStatus>();
        }

        internal static IServiceCollection AddGluttony(this IServiceCollection services)
        {
            return services.AddSingleton<Gluttony>();
        }
    }
}
