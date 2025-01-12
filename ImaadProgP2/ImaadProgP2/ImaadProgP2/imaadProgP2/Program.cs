using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ImaadProgP2.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    await SeedRolesAsync(roleManager);
    await SeedUsersAsync(userManager);
}

app.Run();

async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Lecturer", "Admin", "Programme Coordinator", "Academic Manager", "HR" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
{
    var lecturerEmail = "lecturer@example.com";
    var lecturerPassword = "Lecturer@123";

    if (userManager.Users.All(u => u.Email != lecturerEmail))
    {
        var lecturerUser = new IdentityUser
        {
            UserName = lecturerEmail,
            Email = lecturerEmail,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(lecturerUser, lecturerPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(lecturerUser, "Lecturer");
        }
    }

    var adminEmail = "admin@example.com";
    var adminPassword = "Admin@123";

    if (userManager.Users.All(u => u.Email != adminEmail))
    {
        var adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    var academicManagerEmail = "academicmanager@example.com";
    var academicManagerPassword = "AcademicManager@123";

    if (userManager.Users.All(u => u.Email != academicManagerEmail))
    {
        var academicManagerUser = new IdentityUser
        {
            UserName = academicManagerEmail,
            Email = academicManagerEmail,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(academicManagerUser, academicManagerPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(academicManagerUser, "Academic Manager");
        }
    }

    var programmeCoordinatorEmail = "programmecoordinator@example.com";
    var programmeCoordinatorPassword = "ProgrammeCoordinator@123";

    if (userManager.Users.All(u => u.Email != programmeCoordinatorEmail))
    {
        var programmeCoordinatorUser = new IdentityUser
        {
            UserName = programmeCoordinatorEmail,
            Email = programmeCoordinatorEmail,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(programmeCoordinatorUser, programmeCoordinatorPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(programmeCoordinatorUser, "Programme Coordinator");
        }
    }

    var hrEmail = "hr@example.com";
    var hrPassword = "Hr@123";

    if (userManager.Users.All(u => u.Email != hrEmail))
    {
        var hrUser = new IdentityUser
        {
            UserName = hrEmail,
            Email = hrEmail,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(hrUser, hrPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(hrUser, "HR");
        }
    }
}
