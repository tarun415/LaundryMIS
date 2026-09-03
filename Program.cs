using LaudaryMis.Repositories;
using LaudaryMis.Repositories.Interfaces;
using LaudaryMis.Repository;
using LaudaryMis.Services;
using LaudaryMis.Services.Interfaces;
using LaudaryMis.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Rotativa.AspNetCore;
using System.Data;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();


// Authentication
//
// One cookie per tab slot. Each cookie's Path is that slot's URL prefix, so a
// browser only ever sends slot 0's cookie to "/u/0/..." requests. That is what
// lets two roles stay signed in at once in the same browser: signing in on
// "/u/1" cannot touch the session a tab on "/u/0" is using.
//
// The default scheme is a policy scheme that forwards to whichever slot the
// current request is running under.
var authentication = builder.Services
    .AddAuthentication(TabSlots.PolicySchemeName)
    .AddPolicyScheme(TabSlots.PolicySchemeName, TabSlots.PolicySchemeName, options =>
    {
        options.ForwardDefaultSelector =
            context => TabSlots.SchemeFor(context.CurrentSlot());
    });

for (int slot = 0; slot < TabSlots.Count; slot++)
{
    // Captured per iteration so each handler configures its own slot.
    int currentSlot = slot;

    authentication.AddCookie(TabSlots.SchemeFor(currentSlot), options =>
    {
        // PathBase already carries "/u/{slot}", so these stay slot-relative.
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";

        options.Cookie.Name = TabSlots.CookieNameFor(currentSlot);

        // The browser scopes the cookie to this slot's URLs. This is the part
        // that actually isolates the tabs.
        options.Cookie.Path = TabSlots.CookiePathFor(currentSlot);

        options.ExpireTimeSpan = TimeSpan.FromHours(8);

        options.SlidingExpiration = true;
    });
}

builder.Services.AddAuthorization();
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
//builder.Services.AddScoped<HospitalRepository>();
//builder.Services.AddScoped<HospitalService>();
builder.Services.AddScoped<IAgreementRepository, AgreementRepository>();
builder.Services.AddScoped<IAgreementService, AgreementService>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IWPRRepository, WPRRepository>();
builder.Services.AddScoped<IWPRService, WPRService>();
builder.Services.AddScoped<IWardRepository, WardRepository>();
builder.Services.AddScoped<IWardService, WardService>();
builder.Services.AddScoped<HospitalRepository, HospitalRepository>();
builder.Services.AddScoped<IHospitalService, HospitalService>();
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

// Must run before static files: views emit "~/css/..." as "/u/{slot}/css/...",
// so the slot prefix has to move to PathBase before the file is looked up.
app.UseMiddleware<TabSlotMiddleware>();

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