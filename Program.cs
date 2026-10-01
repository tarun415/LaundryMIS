using LaudaryMis.Repositories;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Repository;
using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Rotativa.AspNetCore;
using System.Data;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// MVC
// Every POST/PUT/DELETE must carry a valid anti-forgery token.
builder.Services.AddControllersWithViews(o =>
    o.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()));


// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";

        options.Cookie.Name = "LaundryMISAuth";

        options.ExpireTimeSpan = TimeSpan.FromHours(8);

        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddAntiforgery(o => o.HeaderName = "RequestVerificationToken");
builder.Services.AddSingleton<LaudaryMis.Helpers.LoginAttemptTracker>();

// Cap request bodies (uploads are validated to 10 MB individually).
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o =>
    o.MultipartBodyLengthLimit = 12 * 1024 * 1024);
// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProviderService, ProviderService>();

builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();
builder.Services.AddScoped<IHospitalService, HospitalService>();

// DB
builder.Services.AddScoped<IDailyService, DailyService>();
builder.Services.AddScoped<IDailyRepository, DailyRepository>();
builder.Services.AddScoped<IAgreementRepository, AgreementRepository>();
builder.Services.AddScoped<IAgreementService, AgreementService>();
builder.Services.AddScoped<IWPRRepository, WPRRepository>();
builder.Services.AddScoped<IWPRService, WPRService>();
builder.Services.AddScoped<IWardRepository, WardRepository>();
builder.Services.AddScoped<IWardService, WardService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IMonthlyBillRepository, MonthlyBillRepository>();
builder.Services.AddScoped<IMonthlyBillService, MonthlyBillService>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IPickUpRepository, PickUpRepository>();
builder.Services.AddScoped<IPickUpService, PickUpService>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IDeliveryChallanRepository, DeliveryChallanRepository>();
builder.Services.AddScoped<IDeliveryChallanService, DeliveryChallanService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IWarningLetterRepository, WarningLetterRepository>();
builder.Services.AddScoped<IWarningLetterService, WarningLetterService>();
builder.Services.AddScoped<IProviderProfileRepository, ProviderProfileRepository>();
builder.Services.AddScoped<IProviderProfileService, ProviderProfileService>();
// 🔥 FIX (IMPORTANT)
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(cs);
});


// Configure QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;  // This is for PDF a 
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles(); 
RotativaConfiguration.Setup(
    app.Environment.WebRootPath,
    "Rotativa");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();