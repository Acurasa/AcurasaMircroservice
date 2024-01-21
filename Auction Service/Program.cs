using Auction_Service.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // provide assembly with class deriving from Automapper profile class Profile
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseAuthorization();

app.MapControllers();
try
{
    Dbinitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}
app.Run();
