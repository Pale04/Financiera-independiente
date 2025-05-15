using FinancieraServer.Interfaces;
using FinancieraServer.ServiceImplementations;
using Serilog;

var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

builder.Services.AddSingleton<SessionService>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<CatalogService>();
builder.Services.AddSingleton<CustomerService>();
builder.Services.AddSingleton<PaymentService>();
builder.Services.AddSingleton<CreditService>();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
Log.Information("Configuration ready for start the server");
builder.Host.UseSerilog();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<SessionService>();
    serviceBuilder.AddServiceEndpoint<SessionService, ISessionService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/SessionService.svc");
    serviceBuilder.AddService<AccountService>();
    serviceBuilder.AddServiceEndpoint<AccountService, IAccountService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/AccountService.svc");
    serviceBuilder.AddService<CatalogService>();
    serviceBuilder.AddServiceEndpoint<CatalogService, ICatalogService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/CatalogService.svc");
    serviceBuilder.AddService<CustomerService>();
    serviceBuilder.AddServiceEndpoint<CustomerService, ICustomerService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/CustomerService.svc");
    serviceBuilder.AddService<PaymentService>();
    serviceBuilder.AddServiceEndpoint<PaymentService, IPaymentService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/PaymentService.svc");
    serviceBuilder.AddService<CreditService>();
    serviceBuilder.AddServiceEndpoint<CreditService, ICreditService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/CreditService.svc");

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});



app.Run();
