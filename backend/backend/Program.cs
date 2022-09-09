using backend;
using backend.Models;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;

bool _needReCache = true;
const string CACHE_KEY = "results";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// i was hosting my own sql database on another computer, i dont really want to open that up to the public so where just going to use an in memory database
builder.Services.AddDbContext<DBContext>(p => p.UseInMemoryDatabase("MyTestDatabase"));
builder.Services.AddEasyCaching(options => { options.UseInMemory("inMemoryCache"); });

// allow connection from anywhere
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// launch swagger so we can easily test the api when developing 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyOrigin");

// create a new entry
app.MapPost("/calc", (DBContext context, IEasyCachingProvider cache, string username, string calculation) =>
{
    Calculation calc = new Calculation
    {
        calculation = calculation,
        user = username,
        timeStamp = DateTime.Now
    };

    try
    {
        // add to list of calculations
        context.Calculations.Add(calc);

        context.SaveChanges();
        _needReCache = true;

        return Results.Ok();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
})
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("Add Entry");

// retireve most recent entries 
app.MapGet("/calc", (DBContext context, IEasyCachingProvider cache) =>
{
    try
    {
        const int NUMBER_OF_ENTRIES = 50;

         // if we dont have cache or data has been changed since last cache then re cache it
        if (!cache.Exists(CACHE_KEY) || _needReCache)
        {
            var result = context.Calculations.Take(NUMBER_OF_ENTRIES).ToArray();

            cache.Set(CACHE_KEY, result, TimeSpan.FromDays(1));
            _needReCache = false;

            return Results.Ok(result);
        }
        else
            return Results.Ok(cache.Get<Calculation[]>(CACHE_KEY).Value);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
})
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("Get Entries");

// delete all of a user's entries from the database
app.MapDelete("/calc", (DBContext context, IEasyCachingProvider cache, string username) =>
{
    try
    {
        // remove all entries created by user
        foreach (var entry in context.Calculations.Where(e => e.user.Equals(username)))
            context.Calculations.Remove(entry);

        context.SaveChanges();
        _needReCache = true;

        return Results.Ok();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
})
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("Delete User's Entries");

app.Run();