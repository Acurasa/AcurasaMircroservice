using Auction_Service.Consumers;
using Auction_Service.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // provide assembly with class deriving from Automapper profile class Profile
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
        {
            o.QueryDelay = TimeSpan.FromSeconds(10);
            o.UsePostgres();
            o.UseBusOutbox();
        });
    x.AddConsumer<AuctionCreatedFaultsConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false)); 
});
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