using BigStore.Data;
using BigStore.Middleware;
using BigStore.Models;
using BigStore.Services;
using BigStore.Utility;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register mail config
builder.Services.AddOptions();                                        // Kích hoạt Options
var mailsettings = builder.Configuration.GetSection("MailSettings");  // đọc config
builder.Services.Configure<MailSettings>(mailsettings);               // đăng ký để Inject
// Add gmail service
builder.Services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;          // Cần xác thực mail mới đăng nhập ở lần tạo tài khoản

});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/AccessDenied";
});

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        // Đọc thông tin Authentication:Google từ appsettings.json
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

        // Thiết lập ClientID và ClientSecret để truy cập API google
        options.ClientId = googleAuthNSection["ClientId"];
        options.ClientSecret = googleAuthNSection["ClientSecret"];
        //https://localhost:7292/LoginToGoogle
        options.CallbackPath = "/LoginToGoogle";
        //options.SaveTokens = true;
    }).AddFacebook(facebookOptions => {
        // Đọc cấu hình
        IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
        facebookOptions.AppId = facebookAuthNSection["AppId"];
        facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
        // Thiết lập đường dẫn Facebook chuyển hướng đến
        facebookOptions.CallbackPath = "/LoginToFacebook";
    })
    //.AddMicrosoftAccount(microsoftOptions => {
    //    // Đọc cấu hình
    //    IConfigurationSection microsoftAuthNSection = builder.Configuration.GetSection("Authentication:Microsoft");
    //    microsoftOptions.ClientId = microsoftAuthNSection["ClientId"];
    //    microsoftOptions.ClientSecret = microsoftAuthNSection["ClientSecret"];
    //    // Thiết lập đường dẫn Facebook chuyển hướng đến
    //    //microsoftOptions.CallbackPath = "/LoginToMicrosoft";
    //})
    //.AddMicrosoftAccount()
    ;

builder.Services.AddSession(optons =>
{
    optons.Cookie.Name = "BigStore";
    optons.IdleTimeout = new TimeSpan(0, 30, 0);
});

//builder.Services.AddSingleton<SecondMiddleware>();
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

// add policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllowEditRole", policybuilder =>
    {
        //dieu kien policy
        policybuilder.RequireAuthenticatedUser();
        policybuilder.RequireRole("Admin");
        policybuilder.RequireClaim("manage", "add");
    });
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMyMiddleware();
//app.UseMiddleware<SecondMiddleware>();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

app.Run();
