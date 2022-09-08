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
app.MapPost("/calcadd", (string username, string calculation) =>
{
    var session = ravenDB.createSession();
    var curTime = DateTime.Now;

    User user = new User
    {
        name = username
    };

    TimeStamp timestamp = new TimeStamp
    {

    };

    Calculation calc = new Calculation
    {
        calculation = calculation,
        user = user,
        timeStamp = timestamp
    };

    // check if we already have the user, if not add
    if (session.Query<User>().Where(x => x.name.Equals(username)).ToList().Count == 0)
        session.Store(user);

    session.Store(calc);
    session.SaveChanges();
})
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError)
    .WithName("Add Entry");

// retireve most recent entries 
app.MapGet("/calcget", (int numberOfEntries) =>
{
    var session = ravenDB.createSession();
    return session.Query<Calculation>().Take(numberOfEntries).ToList();
})
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError)
    .WithName("Get Entries");

// delete all of a user's entries from the database
app.MapDelete("/calcdel", (string username) =>
{
    var session = ravenDB.createSession();
    var userEntries = session.Query<Calculation>().Where(x => x.user.name.Equals(username)).ToList();

    foreach (var i in userEntries)
    {
        session.Delete(i);
    }

    session.SaveChanges();
})
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError)
    .WithName("Delete User's Entries");

app.Run();