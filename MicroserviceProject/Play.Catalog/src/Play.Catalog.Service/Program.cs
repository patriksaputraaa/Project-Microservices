using Play.Catalog.Service;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options => options.SuppressAsyncSuffixInActionNames = false
);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddMongo().AddRepositories().AddServices();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddMongo().AddMongoRepository<Item>("items");

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
