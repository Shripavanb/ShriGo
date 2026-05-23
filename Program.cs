using Microsoft.EntityFrameworkCore;
using ShriGo.Model;
using ShriGo.Pages;
using ShriGo.Pages.Booking;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// ======================================================
// SERVICES
// ======================================================

// Razor Pages + Controllers
builder.Services.AddRazorPages();
builder.Services.AddControllers();


// ======================================================
// DATABASE
// ======================================================

builder.Services.AddDbContext<RideDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureSqlConnection")));


// ======================================================
// SESSION
// ======================================================

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

    options.Cookie.SecurePolicy =
        CookieSecurePolicy.Always;
});


// ======================================================
// EMAIL SETTINGS
// ======================================================

var emailJson =
    builder.Configuration["EmailSettingsJson"];

if (!string.IsNullOrEmpty(emailJson))
{
    var emailSettings =
        JsonSerializer.Deserialize<EmailSettings>(emailJson);

    if (emailSettings != null)
    {
        builder.Services.AddSingleton(emailSettings);
    }
}


// ======================================================
// CUSTOM SERVICES
// ======================================================

builder.Services.AddScoped<EmailService>();


// ======================================================
// SECURITY
// ======================================================

ServicePointManager.SecurityProtocol =
    SecurityProtocolType.Tls12;


var app = builder.Build();


// ======================================================
// ERROR HANDLING
// ======================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}


// ======================================================
// HTTPS
// ======================================================

app.UseHttpsRedirection();


// ======================================================
// STATIC FILES
// ======================================================

app.UseStaticFiles();


// ======================================================
// SECURITY HEADERS
// ======================================================

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "DENY";

    context.Response.Headers["X-Content-Type-Options"] =
        "nosniff";

    context.Response.Headers["Referrer-Policy"] =
        "strict-origin-when-cross-origin";

    context.Response.Headers["Permissions-Policy"] =
        "geolocation=(), microphone=()";

    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self' https: data: 'unsafe-inline' 'unsafe-eval';";

    await next();
});


// ======================================================
// ROUTING
// ======================================================

app.UseRouting();


// ======================================================
// SESSION
// ======================================================

app.UseSession();


// ======================================================
// CUSTOM MIDDLEWARE
// ======================================================

app.UseMiddleware<TrackingMiddleware>();


// ======================================================
// AUTHORIZATION
// ======================================================

app.UseAuthentication();

app.UseAuthorization();


// ======================================================
// ENDPOINTS
// ======================================================

app.MapRazorPages();

app.MapControllers();


app.Run();