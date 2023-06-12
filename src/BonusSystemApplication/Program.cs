using Microsoft.EntityFrameworkCore;
using BonusSystemApplication.Mapper;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.Services;
using BonusSystemApplication.DAL.EF;
using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------- REGISTER SERVICES HERE ---------------------------------------------

builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddTransient<IFormsService, FormsService>();

builder.Services.AddTransient<IFormRepository, FormRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IDefinitionRepository, DefinitionRepository>();
builder.Services.AddTransient<ISignaturesRepository, SignaturesRepository>();
builder.Services.AddTransient<IWorkprojectRepository, WorkprojectRepository>();
builder.Services.AddTransient<IGlobalAccessRepository, GlobalAccessRepository>();
builder.Services.AddTransient<IObjectiveResultRepository, ObjectiveResultRepository>();

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
    app.UseExceptionHandler("/Forms/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Form}/{action=Index}/{id?}");

app.Run();
