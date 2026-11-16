using Ayloul.Areas.Admin.Services;
using Ayloul.Data;
using Ayloul.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuestPDF.Infrastructure;
using System;
using System.Globalization;
using System.Text.Json.Serialization;


namespace Ayloul
{
    public class Program
    {
        public static void Main(string[] args)
        {

            QuestPDF.Settings.License = LicenseType.Community;
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region Database
    
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DbConnection"));
            });




            #endregion

            #region Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options => {

                    // كلمة المرور يجب أن تكون على الأقل 8 أحرف
                    options.Password.RequiredLength = 8;


                    // يجب أن تحتوي كلمة المرور على رقم واحد على الأقل
                    // مثال: Mohammad1
                    options.Password.RequireDigit = true;


                    // يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل
                    // مثال: M
                    options.Password.RequireUppercase = true;


                    // يجب أن تحتوي كلمة المرور على حرف صغير واحد على الأقل
                    // مثال: m
                    options.Password.RequireLowercase = true;


                    // لا يشترط وجود رمز خاص مثل @ أو # أو $
                    options.Password.RequireNonAlphanumeric = false;


                    // يجب أن تحتوي كلمة المرور على عدد معين من الأحرف المختلفة
                    // مثال: Abc123 يحتوي على أحرف مختلفة أكثر من 1
                    options.Password.RequiredUniqueChars = 1;



                    // يمنع استخدام نفس البريد الإلكتروني لأكثر من حساب
                    options.User.RequireUniqueEmail = true;



                    // لا يشترط تأكيد البريد الإلكتروني قبل تسجيل الدخول
                    // إذا كانت true يجب على المستخدم تفعيل الإيميل أولاً
                    options.SignIn.RequireConfirmedEmail = false;


                    // لا يشترط تأكيد الحساب قبل تسجيل الدخول
                    options.SignIn.RequireConfirmedAccount = false;



                    // عدد محاولات تسجيل الدخول الخاطئة قبل قفل الحساب
                    // مثال: بعد 5 كلمات مرور خاطئة يتم قفل الحساب
                    options.Lockout.MaxFailedAccessAttempts = 5;


                    // مدة قفل الحساب بعد تجاوز عدد المحاولات
                    // هنا الحساب يبقى مقفلاً لمدة 30 دقيقة
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);


                    // تفعيل نظام قفل الحساب للمستخدمين الجدد
                    // true = الحسابات الجديدة يمكن أن يتم قفلها
                    options.Lockout.AllowedForNewUsers = true;


                }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMemoryCache();
            builder.Services.ConfigureApplicationCookie(
                option =>
                {
                    option.LoginPath = "/User/Account/Login";
                    option.LogoutPath = "/User/Account/Logout";
                    option.AccessDeniedPath = "/Error/Error";
                    option.Cookie.Name = "Ayloul";
                    option.Cookie.HttpOnly = true;
                    option.ExpireTimeSpan = TimeSpan.FromDays(30);
                    option.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                });

            #endregion




            #region languge
            builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
            builder.Services.Configure<RequestLocalizationOptions>(option =>
            {
                var sup_languge = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar"),
                };
                option.DefaultRequestCulture = new RequestCulture("en");
                option.SupportedCultures = sup_languge;
                option.SupportedUICultures = sup_languge;
            });
            #endregion



                    builder.Services.AddDistributedMemoryCache();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSignalR();

            #region Fro Api
            builder.Services.AddHttpClient("AyloulApi", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiSetting:BaseUrl"]!);
            });

            #endregion

            builder.Services.AddScoped<ApiResponseHandler>();

            builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
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
          
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            #region localization
            var locoption = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locoption.Value);
            #endregion

            app.MapHub<ChatHub>("/chatHub");    

            app.MapControllerRoute(
                            name: "areas",
                            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    app.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");

                    app.Run();
                }
                
    }
}
