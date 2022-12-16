using BonusSystemApplication.Models;
using BonusSystemApplication.Models.Repositories;
using BonusSystemApplication.Models.Repositories;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------- REGISTER SERVICES HERE ---------------------------------------------

builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddTransient<IGenericRepository<User>, GenericRepository<User>>();
builder.Services.AddTransient<IGenericRepository<Workproject>, GenericRepository<Workproject>>();

builder.Services.AddTransient<IGlobalAccessRepository, GlobalAccessRepository>();
builder.Services.AddTransient<IFormRepository, FormRepository>();
builder.Services.AddTransient<IWorkprojectRepository, WorkprojectRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IDefinitionRepository, DefinitionRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        SeedData.Seed(services);
    }
}


//--------------------------------------------- REGISTER MIDDLEWARE HERE ---------------------------------------------


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// -------------------------------------------------- UNTILL HERE --------------------------------------------------

app.Run();