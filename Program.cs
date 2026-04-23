using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;
using ShriGo.Pages;
using Twilio.TwiML.Voice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();
//Database connection string
builder.Services.AddDbContext<RideDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")) );

//Session
builder.Services.AddDistributedMemoryCache();//Required for Sesssion timeout
builder.Services.AddSession(options =>
{ 
 options.IdleTimeout = TimeSpan.FromMinutes(20);//Set Session timeout 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential= true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Configure the HTTP request pipeline


//var path = Path.Combine(AppContext.BaseDirectory, "google-credentials.json");

//Environment.SetEnvironmentVariable(
//    "GOOGLE_APPLICATION_CREDENTIALS",
//    path
//);

app.UseSession(); // Enable session middleware

//Number of Site visitors
app.UseMiddleware<TrackingMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");
//});

app.MapRazorPages();
app.MapControllers();

app.Run();
