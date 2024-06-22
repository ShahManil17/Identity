using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerceWithIdentity.Data;
var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("ECommerceWithIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'ECommerceWithIdentityContextConnection' not found.");


builder.Services.AddDbContext<ECommerceWithIdentityContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ECommerceWithIdentityContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EditUser", policy => policy.RequireClaim("Update_user"));
});

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Set your login path here
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
