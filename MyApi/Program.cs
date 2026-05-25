using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.Run();