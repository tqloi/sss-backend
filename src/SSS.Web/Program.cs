using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.HttpOverrides;
using SSS.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = false;
});
builder.Services.AddCoreInfrastructure(builder.Configuration);
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p
        //.WithOrigins(
        //     "http://localhost:5173",   // Vite dev server HTTP
        //     "https://localhost:5173",  // Vite dev server HTTPS
        //     "http://localhost:4200",   // Angular dev server HTTP
        //     "https://localhost:4200"   // Angular dev server HTTPS
        // )
        .AllowAnyOrigin()
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
    //o.KnownNetworks.Clear();
    //o.KnownProxies.Clear();
});

builder.Services.AddProblemDetails();
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

//// Seed Database
//using (var scope = app.Services.CreateScope())
//{
//    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//    var seeder = new DataSeeder(ctx, roleManager, userManager);
//    await seeder.SeedAllAsync();
//}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseFastEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseSwaggerGen();

app.Run();
