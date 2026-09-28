using System.Text.Json;
using FluentValidation;
using TestJobSys;
using TestJobSys.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);


builder.Services.AddSingleton<ParserFactory>();
builder.Services.AddScoped<IValidator<CheckRequest>, ValidatorJson>();


var connectionString = "Host=postgres;Port=5432;Database=postgres;Username=postgres;Password=postgres";
builder.Services.AddScoped<ElementRepository>(_ => new ElementRepository(connectionString));


builder.Services.AddScoped<UseCasesSearch>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "v1"));


app.UseHttpsRedirection();


app.MapPost("/check",
    async (CheckRequest request, IValidator<CheckRequest> validatorJson, UseCasesSearch casesSearch) =>
    {
        var result = validatorJson.Validate(request);
        if (!result.IsValid)
        {
            return Results.Json(CheckResponse.Fail($"{result.Errors[0].ErrorCode}", string.Empty));
        }

        CheckResponse? response;
        try
        {
            response = await casesSearch.Search(request);
        }

        catch (Exception e)
        {
            return Results.Json(CheckResponse.Fail("INTERNAL_ERROR", e.Message), statusCode: 500);
        }


        return Results.Ok(response);
    });


app.Run();


public record CheckRequest(
    string Selector,
    string Attribute,
    string UrlB64,
    string EncryptedTextBytesB64,
    string KeyBytesB64,
    string PageB64
);