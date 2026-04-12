using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();//Required for Sesssion timeout

builder.Services.AddDbContext<RideDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")) );

builder.Services.AddSession(options =>
{ 
 options.IdleTimeout = TimeSpan.FromSeconds(90);//Set Session timeout 
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
app.UseSession(); // Enable session middleware

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
