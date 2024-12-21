using WorkCalendarik.Domain.Database.ModelsDb;
using WorkCalendarik.Domain.Database.Storage;
using WorkCalendarik.Domain.Interfaces;
using WorkCalendarik.Service.Interfaces;
using WorkCalendarik.Service.Realizations;

namespace WorkCalendarik;

public static class Initializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseStorage<UserDb>, UserStorage>();
        services.AddScoped<IBaseStorage<BronCalendarDb>, BronCalendarStorage>();
    }
    
    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IBronCalendarService, BronCalendarService>();
        
        services.AddControllersWithViews()
            .AddDataAnnotationsLocalization()
            .AddViewLocalization();
    }
}
