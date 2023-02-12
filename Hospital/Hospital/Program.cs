using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.Repositories;
using Hospital.Services.Interfaces;
using Hospital.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                            .AddRoles<IdentityRole<Guid>>()
                            .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPatientService, PatientService>();
builder.Services.AddTransient<IPatientRepository, PatientRepository>();
builder.Services.AddTransient<IExaminationService, ExaminationService>();
builder.Services.AddTransient<IExaminationRepository, ExaminationRepository>();
builder.Services.AddTransient<IVisitService, VisitService>();
builder.Services.AddTransient<IVisitRepository, VisitRepository>();
// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
