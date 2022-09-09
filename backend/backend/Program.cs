using backend;
using backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// i was hosting my own sql database on another computer, i dont really want to open that up to the public so where just going to use an in memory database
builder.Services.AddDbContext<DBContext>(p => p.UseInMemoryDatabase("MyTestDatabase"));

var app = builder.Build();

// cache isnt really needed for in memory database, however i was using this with an actualy sql database earlier so i added it
var cache = new SimpleCache("cache");

// launch swagger so we can easily test the api when developing 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// create a new entry
app.MapPost("/calc", (DBContext context, string username, string calculation) =>
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
        cache.setRecahce(true);

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
app.MapGet("/calc", (DBContext context) =>
{
    try
    {
        const int NUMBER_OF_ENTRIES = 50;

        // if we dont have cache or data has been changed since last cache then re cache it
        if (!cache.hasCache() || cache.needRecahce())
        {
            var result = context.Calculations.Take(NUMBER_OF_ENTRIES).ToArray();

            cache.updateCache(result);
            cache.setRecahce(false);

            return Results.Ok(result);
        }
        else
            return Results.Ok(cache.getCached());

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
app.MapDelete("/calc", (DBContext context, string username) =>
{
    try
    {
        // remove all entries created by user
        foreach (var entry in context.Calculations.Where(e => e.user.Equals(username)))
            context.Calculations.Remove(entry);

        context.SaveChanges();
        cache.setRecahce(true);

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