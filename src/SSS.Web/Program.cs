using FastEndpoints;
using FastEndpoints.Swagger;
using Hangfire;
using Microsoft.AspNetCore.HttpOverrides;
using SSS.Infrastructure;
using SSS.Infrastructure.Realtime;
using SSS.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = false;
});

var secretAppsettingsPath = "/secrets/sss-appsettings/sss-appsettings";
if (File.Exists(secretAppsettingsPath))
    builder.Configuration.AddJsonFile(secretAppsettingsPath, optional: false, reloadOnChange: true);

var gcpKeyPath = "/secrets/sss-gcp-key/sss-gcp-key";
if (File.Exists(gcpKeyPath))
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", gcpKeyPath);

builder.Services.AddCoreInfrastructure(builder.Configuration);
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p
        .WithOrigins(
             "https://studysense-frontend.vercel.app",
             "http://localhost:3000", 
             "https://localhost:3000" 
         )
        .AllowAnyHeader()
        .AllowAnyMethod());
});

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenLocalhost(5097, o => o.Protocols = HttpProtocols.Http1AndHttp2); // HTTP
//    options.ListenLocalhost(7097, o =>
//    {
//        o.Protocols = HttpProtocols.Http1AndHttp2; // Hỗ trợ cả 2
//        o.UseHttps(); // HTTPS
//    });
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    o.ForwardLimit = 2;
    o.RequireHeaderSymmetry = false;
    o.KnownNetworks.Clear();
    o.KnownProxies.Clear();
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Seed Database
//using (var scope = app.Services.CreateScope())
//{
//    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//    var seeder = new DataSeeder(ctx, roleManager, userManager);
//    await seeder.SeedAllAsync();
//}

app.UseExceptionHandler();
app.UseForwardedHeaders();

// Avoid redirect surprises in local development where frontend/backend are mixed HTTP/HTTPS.
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// ── Hangfire dashboard (dev only) ─────────────────────────────────────────────
if (app.Environment.IsDevelopment())
    app.UseHangfireDashboard("/hangfire");

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");
app.MapHub<UserGamificationHub>("/hubs/user-gamification");
app.UseFastEndpoints(c =>
{
    c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseSwaggerGen();

app.Run();
