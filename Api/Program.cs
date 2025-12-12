using Api.Extensions;
using Application.Queries.Store.ProductQueries;
using Infrastructure.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddDatabase(configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwagger();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureMvc();
builder.Services.AddHealthChecks();
builder.Services.AddValidators();
builder.Services.AddMapster();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllProducts).Assembly));


// Add Database
builder.Services.AddDatabase(configuration);
// add services 
builder.Services.AddApplicationServices(configuration);

// add repos
builder.Services.AddRepositories();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureExceptionHandler();
app.ConfigureCors();
app.ConfigureSwagger(configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
