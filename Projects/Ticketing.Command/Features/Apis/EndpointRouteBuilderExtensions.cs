namespace Ticketing.Command.Features.Apis;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapMinimalApisEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var minimalApis = endpointRouteBuilder.ServiceProvider.GetServices<IMinimalApi>();

        foreach (var minimalApi in minimalApis)
        {
            minimalApi.AddEndpoints(endpointRouteBuilder);
        }

        return endpointRouteBuilder;
    }
}
