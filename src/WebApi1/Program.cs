using Elastic.Apm.NetCoreAll;
using Prometheus;
using WebApi1.Diagnostics.Aspects;
using WebApi1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();
builder.Services.AddScoped<TestWorker>();
builder.Services.AddSingleton<TraceAspectMethodAround>();

var app = builder.Build();

AspectsFactory.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAllElasticApm(builder.Configuration);
app.UseMetricServer("/api/status/prometheus");

app.UseAuthorization();

app.MapControllers();

app.Run();


//For integration tests
public partial class Program { }
