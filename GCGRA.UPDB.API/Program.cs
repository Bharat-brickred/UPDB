using System.ComponentModel.DataAnnotations;
using GCGRA.UPDB.API.RequestModel;
using GCGRA.UPDB.Application.Features.Blob.Commands;
using GCGRA.UPDB.Application.Features.Players.Queries;
using GCGRA.UPDB.Core.Entities;
using GCGRA.UPDB.Core.Interfaces;
using GCGRA.UPDB.Infrastructure.Repositories;
using GCGRA.UPDB.Infrastructure.Services;
using MediatR;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add MediatR to the container
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadJsonToBlobCommand).Assembly));

// Azure Blob Storage service configuration
// Add MediatR to the container
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadJsonToBlobCommand).Assembly));

// Add services to the container.
builder.Services.AddScoped<IPlayerRepository>(provider =>
    new PlayerRepository(
        builder.Configuration.GetValue<string>("AzureTableStorage:ConnectionString"),
        builder.Configuration.GetValue<string>("AzureTableStorage:TableName")
    )
);

builder.Services.AddScoped<IBlobStorageService>(provider =>
    new BlobStorageService(
        builder.Configuration.GetValue<string>("AzureBlobStorage:ConnectionString"),
        builder.Configuration.GetValue<string>("AzureBlobStorage:ContainerName")
    )
);

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});


// Add Swagger/OpenAPI 3.0 support
builder.Services.AddEndpointsApiExplorer(); // Register the minimal API explorer
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gaming Regulator API",
        Version = "v1",
        Description = "API documentation for the Gaming Regulator system."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    // Optionally, you can customize how the request body looks in Swagger
    options.MapType<UploadRequest>(() => new OpenApiSchema { Type = "object" });
});
var app = builder.Build();

app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gaming Regulator API");
    c.RoutePrefix = string.Empty; // Swagger UI accessible at the root URL (http://localhost:5000)
});
// Configure the HTTP request pipeline.
app.MapGet("/products", async (IMediator mediator) =>
{
    var query = new GetAllPlayersQuery();
    var products = await mediator.Send(query);
    return Results.Ok(products);
});

app.MapGet("/players/{doc-id}", async (int docIdn, IMediator mediator) =>
{
    var query = new GetPlayerByIdQuery(docIdn);
    var product = await mediator.Send(query);
    return product is not null ? Results.Ok(product) : Results.NoContent();
}).WithMetadata(new ApiVersionAttribute("1.0"))
.WithMetadata(new EndpointSummaryAttribute("Retrieve Self-exclusion status of Players by Document ID"));
// POST endpoint to store JSON data in Blob Storage
app.MapPost("/players", async (UploadRequest request, IMediator mediator) =>
{
    var validationResults = ValidatePlayers(request.players);

    if (validationResults.Any())
    {
        return Results.BadRequest(validationResults);
    }

    var blobName = $"players_{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}.json"; // Unique blob name

    // Create the UploadJsonToBlobCommand
    var command = new UploadJsonToBlobCommand(request.players, blobName);

    // Send the command using MediatR
    var uploadedBlobUri = await mediator.Send(command);

    return Results.Created("",new { Message = "Players created successfully", BlobUri = uploadedBlobUri });
})
    .WithMetadata(new ApiVersionAttribute("1.0"))
.WithMetadata(new EndpointSummaryAttribute("Add one or multiple players"));

app.Run();

// Helper method for validating players
List<ValidationResult> ValidatePlayers(List<Player> players)
{
    var validationResults = new List<ValidationResult>();

    foreach (var player in players)
    {
        var playerValidationContext = new ValidationContext(player);
        Validator.TryValidateObject(player, playerValidationContext, validationResults, true);
    }

    return validationResults;
}
