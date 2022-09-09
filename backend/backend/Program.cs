using backend;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// launch swagger so we can easily test the api when developing 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// create an instance of our raven DB
var ravenDB = new RavenDB(builder.Configuration.GetSection("Database"));
var calcCache = new SimpleCache("calcCache");

ravenDB.MakeSureExists();

// create a new entry
app.MapPost("/calc", (string username, string calculation) =>
{
    var curTime = DateTime.Now;
 
    User user = new User
    {
        name = username
    };

    TimeStamp timestamp = new TimeStamp
    {
        month = curTime.Month,
        date = curTime.Day,
        hour = curTime.Hour,
        minute = curTime.Minute
    };

    Calculation calc = new Calculation
    {
        calculation = calculation,
        user = user,
        timeStamp = timestamp
    };

    try
    {
        var session = ravenDB.createSession();

        // check if we already have the user, if not add
        if (session.Query<User>().Where(x => x.name.Equals(username)).ToList().Count == 0)
            session.Store(user);

        session.Store(calc);
        session.SaveChanges();
        
        // we only update the cache if we changed something
        calcCache.setRecahce(true);
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
app.MapGet("/calc", () =>
{
    try
    {
        // if we dont have cache or data has been changed since last cache then re cache it
        if (!calcCache.hasCache() || calcCache.needRecahce())
        {
            var session = ravenDB.createSession();
            var result = session.Query<Calculation>().Take(20).ToList();

            calcCache.updateCache(result);
            calcCache.setRecahce(false);

            return Results.Ok(result);
        }
        else
            return Results.Ok(calcCache.getCached());
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
app.MapDelete("/calc", (string username) =>
{
    try
    {
        var session = ravenDB.createSession();
        var userEntries = session.Query<Calculation>().Where(x => x.user.name.Equals(username)).ToList();

        // remove a user' calculations
        foreach (var i in userEntries)
            session.Delete(i);

        session.SaveChanges();
        calcCache.setRecahce(true);

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