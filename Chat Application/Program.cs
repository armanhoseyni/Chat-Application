using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DB;
using Chat_Application.Areas.Identity.Data;
using Chat_Application.Hubs;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Chat_ApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'Chat_ApplicationContextConnection' not found.");

builder.Services.AddDbContext<Chat_ApplicationContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Chat_ApplicationContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
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
    pattern: "{controller=ChatRoom}/{action=PublicChat}/{id?}");
app.MapHub<ChatRoom>("/PChat");

app.Run();
