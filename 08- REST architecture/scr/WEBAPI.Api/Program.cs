using WEBAPI.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddApiControllers();
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddCors(builder.Configuration);

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddMapper(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddVersioning();
builder.Services.AddSwagger();

var app = builder.Build();


app.AddDeveloperHttpRequestMiddleware();
app.AddDbMigrateAndSeedData(builder.Configuration);
app.AddHttpRequestMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAppSwagger();
app.AddWebApiEndpoints();

app.Run();

public partial class Program { }