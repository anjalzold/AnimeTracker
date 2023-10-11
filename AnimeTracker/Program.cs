using AnimeTracker.Data;
using AnimeTracker.Data.Repositories.Implementation;
using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Data.Repositories.Interface.Base;
using AnimeTracker.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

//------------------------[ Add services to the container ]-----------------------------
void ConfigureServices(IServiceCollection services)
{
    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    RegisterElements(services);
}


//--------------------------------------------------------------------------------------

//--------------------------[ Register Elements Here ]---------------------------------- 
void RegisterElements(IServiceCollection services)
{

 
    AddDbContext(services);
    RegisterRepositories(services);
}
//--------------------------------------------------------------------------------------

//-------------------------------[ Add DbContext ]--------------------------------------
void AddDbContext(IServiceCollection services)
{
    services.AddDbContext<AppDbContext>(options =>
    {
        //---------------------------[ Database Connection ]------------------------------------
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        //--------------------------------------------------------------------------------------
    });
}
//--------------------------------------------------------------------------------------


//-------------------------[ Register Repository Here ]---------------------------------
void RegisterRepositories(IServiceCollection services)
{
    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
    services.AddTransient<IAnimeRepository, AnimeRepository>();
    services.AddTransient<ICategoryRepository, CategoryRepository>();
    services.AddTransient<IFileHelper, FileHelperImplementation>();
    services.AddTransient<IAnimeCategory, AnimeCategoryRepository>();


    
    
}
//--------------------------------------------------------------------------------------

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


// app.MapControllerRoute(
//     name: "Anime",
//     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();