using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using WebApp.Configs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // địa chỉ Redis
});
builder.Services.AddHttpClient();

builder.Services.Configure<ApiConfigs>(builder.Configuration.GetSection("ApiConfigs"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<ApiConfigs>>().Value);

// thêm dịch vụ authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = "AdminCookie";

    //option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie("User", option =>
    {
        option.LoginPath = "/Loginclient/Loginclient"; // Đường dẫn khi user chưa đăng nhập
    })


   .AddCookie("AdminCookie", options =>
   {
       options.LoginPath = "/admin/LoginAdmin/Login";
       options.AccessDeniedPath = "/admin/phanquyen/index";
       options.Cookie.Name = "AdminCookie";
       options.Cookie.SecurePolicy = CookieSecurePolicy.None;  // ✅ Đừng để là Always nếu chạy HTTP
       options.Cookie.SameSite = SameSiteMode.Lax;             // ✅ Tránh bị chặn
       options.ExpireTimeSpan = TimeSpan.FromMinutes(2880); // đang để 1phut test, muốn thì sửa lại
       options.SlidingExpiration = true;
   }
  );
// Thêm chính sách "AdminPolicy"
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("AdminCookie");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });

    options.AddPolicy("HanhChinhPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("AdminCookie");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("HanhChinh");
    });

    options.AddPolicy("KyThuatPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("AdminCookie");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("KyThuat");
    });
    options.AddPolicy("DirectorPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("AdminCookie");
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Director");
    });
});


builder.Services.AddScoped<AuthorizeTokenAttribute>();

// Thêm dịch vụ sử dụng Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddJavaScriptBundle("/js/bundle.js", "Content2/js/jquery.min.js",
        "Content2/js/jquery-migrate-3.0.1.min.js",
        "Content2/js/bootstrap.min.js",
        "Content2/js/jquery.animateNumber.min.js",
        "Content2/js/jquery.easing.1.3.js",
        "Content2/js/jquery.magnific-popup.min.js",
        "Content2/js/jquery.stellar.min.js",
        "Content2/js/jquery.waypoints.min.js",
        "Content2/js/main.js",
        "Content2/js/owl.carousel.min.js",
        "Content2/js/popper.min.js",
        "Content2/js/scrollax.min.js",
        "Content2/js/main_1.js"
        );

    pipeline.AddCssBundle("/css/bundle.css",
        "Content2/css/animate.css",
        "Content2/css/owl.carousel.min.css",
        "Content2/css/owl.theme.default.min.css",
        "Content2/css/magnific-popup.css",
        "Content2/css/flaticon.css",
        "Content2/css/style.css"
        );

    pipeline.AddJavaScriptBundle("/js/bundle_admin.js",
        "Content/Clients/plugins/jquery/jquery.min.js",
       "Content/Clients/plugins/bootstrap/js/bootstrap.bundle.min.js",
       "Content/Clients/plugins/select2/js/select2.full.min.js",
       "Content/Clients/plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js",
       "Content/Clients/plugins/moment/moment.min.js",
       "Content/Clients/plugins/inputmask/jquery.inputmask.min.js",
       "Content/Clients/plugins/daterangepicker/daterangepicker.js",
       "Content/Clients/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js",
       "Content/Clients/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
       "Content/Clients/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
       "Content/Clients/plugins/bs-stepper/js/bs-stepper.min.js",
       "Content/Clients/plugins/dropzone/min/dropzone.min.js",
       "Content/Clients/dist/js/adminlte.min.js",
       "Content/Clients/dist/js/demo.js"
       );

    pipeline.AddCssBundle("/css/bundle_admin.css",
       "Content/Clients/plugins/fontawesome-free/css/all.min.css",
       "Content/Clients/plugins/daterangepicker/daterangepicker.css",
       "Content/Clients/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
       "Content/Clients/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css",
       "Content/Clients/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
       "Content/Clients/plugins/select2/css/select2.min.css",
       "Content/Clients/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css",
       "Content/Clients/plugins/bootstrap4-duallistbox/bootstrap-duallistbox.min.css",
       "Content/Clients/plugins/bs-stepper/css/bs-stepper.min.css",
       "Content/Clients/plugins/dropzone/min/dropzone.min.css",
       "Content/Clients/dist/css/adminlte.min.css"
       );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();

app.UseWebOptimizer();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
    }
});
app.UseStaticFiles();

app.UseRouting();

app.Use(async (ctx, next) =>
{
    var cookie = ctx.Request.Cookies["AdminCookie"];
    Console.WriteLine($"AdminCookie: {cookie}");
    Console.WriteLine("IsAuthenticated: " + ctx.User.Identity?.IsAuthenticated);
    await next();
});


app.UseAuthentication();

app.UseAuthorization();

// Map area trước
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=HomeAdmin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
       pattern: "{controller=HomeGuest}/{action=Index}/{id?}");



app.Run();
