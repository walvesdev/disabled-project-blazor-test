using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sindvamb.Domain.Entities;
using Sindvamb.Repository.Contexts;
using Sindvamb.Repository.Dapper;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Repository.ORM;
using Sindvamb.Repository.Repositories;
using Sindvamb.Repository.UnitsOfWork;

namespace Sindvamb.Repository.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(f => new DapperDbSession());
            services.AddTransient(typeof(IDapperRepository<,>), typeof(DapperRepository<,>));

            services.AddDbContext<SindvambDbContext>(options =>
            {
                options
                    .UseLazyLoadingProxies(opt => opt.IgnoreNonVirtualNavigations(true))
                    .ConfigureWarnings(b =>
                    {
                        b.Throw(CoreEventId.NavigationLazyLoading);
                        b.Throw(CoreEventId.DetachedLazyLoadingWarning);
                        b.Throw(CoreEventId.LazyLoadOnDisposedContextWarning);
                    })
                    .UseNpgsql(configuration.GetConnectionString("Sindvamb"), options =>
                        options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                    
            }, ServiceLifetime.Transient);

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(ISindvambUnitOfWork), typeof(SindvambUnitOfWork));

            services.AddScoped(typeof(IRepository<,,>), typeof(Repository<,,>));
            services.AddScoped(typeof(ISindvambRepository<,>), typeof(SindvambRepository<,>));


            return services;
        }
    }
}