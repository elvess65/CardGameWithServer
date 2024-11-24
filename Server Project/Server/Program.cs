using Server.Services;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(o => o.SerializerSettings.ContractResolver = new DefaultContractResolver());
builder.Services.AddScoped<IUpdaterService, UpdaterService>();
builder.Services.AddScoped<IDataService, PlayerService>();

var app = builder.Build();
if (app.Environment.IsDevelopment()) { }

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();