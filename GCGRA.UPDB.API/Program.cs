using GCGRA.UPDB.API.RequestModel;
using GCGRA.UPDB.Application.Features.Blob.Commands;
using GCGRA.UPDB.Application.Features.Players.Queries;
using GCGRA.UPDB.Core.Interfaces;
using GCGRA.UPDB.Infrastructure.Repositories;
using GCGRA.UPDB.Infrastructure.Services;
using MediatR;
using Microsoft.OpenApi.Models; // Add this using directive
// Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add MediatR to the container
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UploadJsonToBlobCommand).Assembly));


// Add services to the container.
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
// Azure Blob Storage service configuration
builder.Services.AddSingleton(new BlobStorageService(
    builder.Configuration.GetValue<string>("AzureBlobStorage:ConnectionString"),
    builder.Configuration.GetValue<string>("AzureBlobStorage:ContainerName")
));

// Add Swagger/OpenAPI 3.0 support
builder.Services.AddEndpointsApiExplorer(); // Register the minimal API explorer
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Clean Architecture API",
        Version = "v1",
        Description = "API for uploading JSON data to Azure Blob Storage using Mediator Pattern."
    });
    // Optionally, you can customize how the request body looks in Swagger
    options.MapType<UploadRequest>(() => new OpenApiSchema { Type = "object" });
});
var app = builder.Build();


app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture API v1");
    c.RoutePrefix = string.Empty; // Swagger UI accessible at the root URL (http://localhost:5000)
});
// Configure the HTTP request pipeline.
app.MapGet("/products", async (IMediator mediator) =>
{
    var query = new GetAllPlayersQuery();
    var products = await mediator.Send(query);
    return Results.Ok(products);
});

app.MapGet("/products/{id}", async (int id, IMediator mediator) =>
{
    var query = new GetPlayerByIdQuery(id);
    var product = await mediator.Send(query);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

// POST endpoint to store JSON data in Blob Storage
app.MapPost("/upload-json", async (UploadRequest request, IMediator mediator) =>
{
    var players = request.players;
    var blobName = $"json-{Guid.NewGuid()}.json"; // Unique blob name

    // Create the UploadJsonToBlobCommand
    var command = new UploadJsonToBlobCommand(players, blobName);

    // Send the command using MediatR
    var uploadedBlobUri = await mediator.Send(command);

    return Results.Ok(new { Message = "File uploaded successfully", BlobUri = uploadedBlobUri });
});

app.Run();
