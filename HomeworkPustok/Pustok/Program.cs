using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using Pustok.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PustokDbContext>(opt =>
{
    opt.UseSqlServer("Server=DESKTOP-8VNLF73\\SQLEXPRESS;Database=PustokDb;Trusted_Connection=True");
});
builder.Services.AddScoped<LayoutService>();

builder.Services.AddCookiePolicy(opts => {
	opts.CheckConsentNeeded = ctx => false;
	opts.OnAppendCookie = ctx => {
		ctx.CookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(20);
	};
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength= 8;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<PustokDbContext>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
