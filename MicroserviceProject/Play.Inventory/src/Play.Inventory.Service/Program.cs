using Play.Common.Service.MongoDB;
using Play.Inventory.Service.Entities;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Play.Inventory.Service.Clients;

var builder = WebApplication.CreateBuilder(args);
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
builder.Services.AddControllers(
    options => options.SuppressAsyncSuffixInActionNames = false
);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMongo().AddMongoRepository<InventoryItem>("inventoryItems");
builder.Services.AddHttpClient<CatalogClient>(client => {
    client.BaseAddress = new Uri("https://localhost:7115",)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
