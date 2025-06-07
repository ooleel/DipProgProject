using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeniorLearnWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

//1. App DbContext
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//2. Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

/* ADD THIS?
//3. MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
*/

/* ADD THIS?
//4. Domain services/repos
builder.Services.AddScoped<IMemberService, MemberService>(); //1 instance per HTTP request
builder.Services.AddScoped<ILessonService, LessonService>();

Depends on interfaces: repository (like IMemberRepository) and service (like IMemberService) improving single responsibility and organisation.
Their implementation classes (like MemberRepository and MemberService) are registered in the DI container and allow to handle Id operations like login and registration handling.
*/

//5. Error pages
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

//6. Authentication
app.UseAuthorization();

app.MapStaticAssets();

//7. Endpoints: for all MVC controllers 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
