using Play.Catalog.Service;
using MongoDB.Driver;
using Play.Catalog.Service.Settings;
using Play.Catalog.Service.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options => options.SuppressAsyncSuffixInActionNames = false
);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddMongo().AddRepositories().AddServices();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var serviceSettings= builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
builder.Services.AddSingleton(ServiceProvider => {
    var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
    return mongoClient.GetDatabase(serviceSettings.ServiceName);
});

builder.Services.AddSingleton<IItemRepository, ItemRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
