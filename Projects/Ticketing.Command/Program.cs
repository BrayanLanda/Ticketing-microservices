using MediatR;
using Ticketing.Command.Application;
using Ticketing.Command.Infrastructure;
using static Ticketing.Command.Features.Tickets.TicketCreate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


app.MapPost("api/ticket", async (TicketCreateRequest request, IMediator mediator, CancellationToken cancellationToken) =>
{
    var command = new TicketCreateCommand(request);
    var result = await mediator.Send(command, cancellationToken);
    return result
        ? Results.Ok(result)
        : Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Could not create the ticket",
            detail: "The ticket could not be saved. Check the application logs for the underlying error."
        );
}).WithName("CreateTicket");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
