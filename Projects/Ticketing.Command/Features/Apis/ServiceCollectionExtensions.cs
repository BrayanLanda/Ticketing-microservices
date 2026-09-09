using System.Reflection;

namespace Ticketing.Command.Features.Apis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMinimalApis(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var mininalApis = currentAssembly.GetTypes()
            .Where(t => typeof(IMinimalApi).IsAssignableFrom(t) && t != typeof(IMinimalApi) && !t.IsAbstract && t.IsPublic);

        foreach (var minimalApi in mininalApis)
        {
            services.AddSingleton(typeof(IMinimalApi), minimalApi);
        }

        return services;
    }
}
