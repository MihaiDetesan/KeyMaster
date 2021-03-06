var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/Key", () =>
{
    //var forecast = Enumerable.Range(1, 5).Select(index =>
    //   new WeatherForecast
    //   (
    //       DateTime.Now.AddDays(index),
    //       Random.Shared.Next(-20, 55),
    //       summaries[Random.Shared.Next(summaries.Length)]
    //   ))
    //    .ToArray();
    //return forecast;
})
.WithName("AddKey");

app.MapPost("/person", () =>
{
})
.WithName("AddPerson");

app.MapPut("/key", () =>
{
})
.WithName("TransferKey");

app.MapGet("/keys", () =>
{

}).WithName("GetMyKeys");

app.Run();

