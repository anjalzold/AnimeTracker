using AnimeTracker.Data;
using AnimeTracker.Data.Repositories.Implementation;
using AnimeTracker.Data.Repositories.Implementation.Base;
using AnimeTracker.Data.Repositories.Interface;
using AnimeTracker.Data.Repositories.Interface.Base;
using AnimeTracker.Models;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);





ConfigureServices(builder.Services);

//------------------------[ Add services to the container ]-----------------------------
void ConfigureServices(IServiceCollection services)
{
    services.AddNotyf(config=> { config.DurationInSeconds = 10;config.IsDismissable = true;config.Position = NotyfPosition.BottomRight; });
    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    builder.Services.AddSession();

    // services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //     .AddCookie(
    //         option =>
    //         {
    //             option.LoginPath = "/Home/Login";
    //             option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    //         }
    //         
    // )
    // ;
    
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
    services.AddTransient<IUsers, UserRepository>();



    
    
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

app.UseNotyf();


app.UseHttpsRedirection();
app.UseStaticFiles();

// app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();
app.UseSession();


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