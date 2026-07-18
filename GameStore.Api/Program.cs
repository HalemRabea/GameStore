using GameStore.Api.Data;
using GameStore.Api.EndPoints;
using GameStore.Api.MiddleWares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
builder.AddGameStoreDb();
var app = builder.Build();
app.UseRequestLogging();
app.MapGamesEndpoints();
app.MapGenresEndpoints();
app.MigrateDatabase();
app.Run();
