using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Radin.Application.Interfaces.Contexts;


using Radin.Infrastructure.IdentityConfigs;

//using Radin.Application.Services;

using DotNetEnv;



Env.Load();
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var configuration = builder.Configuration;
var services = builder.Services;
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
//builder.Services.AddScoped<IPriceFeeDataBaseContext, PriceFeeDataBaseContext>();


builder.Services.AddIdentityService(builder.Configuration);



//builder.Services.AddScoped<IPolicyRequirementsService, PolicyRequirementsService>();

//builder.Services.AddAuthorization(options =>
//{

//    options.AddPolicy("DefaultPolicy", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//    });
//    var serviceProvider = services.BuildServiceProvider();
//    var policyRequirementsService = serviceProvider.GetRequiredService<IPolicyRequirementsService>();
//    var policyRequirements = policyRequirementsService.GetPolicyRequirements();

//    foreach (var requirement in policyRequirements)
//    {
//        options.AddPolicy(requirement.PolicyName, policy =>
//        {
//            policy.RequireClaim(requirement.ClaimType, requirement.ClaimValue);
//        });
//    }
//});




//----------------------------------------------------------------//
//builder.Services.AddInfrastructureServices(builder.Configuration);
//builder.Services.AddScoped<ChannelliumMapper, ChannelliumMapper>();
//builder.Services.AddScoped<SwediMapper, SwediMapper>();
//builder.Services.AddScoped<SwediMaxMapper, SwediMaxMapper>();
//builder.Services.AddScoped<PlasticMapper, PlasticMapper>();
//builder.Services.AddScoped<ReportPdfService, ReportPdfService>();


var allowedOrigin1 = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN1");
var allowedOrigin2 = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN2");
var allowedOrigin3 = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN3");
var allowedOrigin4 = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN4");
var allowedOrigin5 = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN5");
var allowedOrigin6 = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN6");
var connection1 = Environment.GetEnvironmentVariable("CONNECTION_RADINGH");
var connection2 = Environment.GetEnvironmentVariable("CONNECTION_RADINPRICE");
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCors", builder =>
    {
        builder.WithOrigins(allowedOrigin1, allowedOrigin2, allowedOrigin3, allowedOrigin4, allowedOrigin5, allowedOrigin6)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()
               .SetIsOriginAllowedToAllowWildcardSubdomains(); // Optional: Allow subdomains

    });
});


builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(connection1);//, b => b.MigrationsAssembly("EndPoint.Site")
});
//builder.Services.AddDbContext<PriceFeeDataBaseContext>(options =>
//{
//    options.UseSqlServer(connection2);//, b => b.MigrationsAssembly("EndPoint.Site")
//});

builder.WebHost.ConfigureKestrel(options =>
{
    options.AllowSynchronousIO = true;
});
//builder.Services.AddHostedService<CheckExpirationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}


app.UseCors("MyCors");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    // Map attribute-routed controllers
    endpoints.MapControllers();

    // Map conventional routes
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller}/{action}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
//app.MapFallbackToFile("index.html"); ;

app.Run();
