using FinancieraServer.Interfaces;
using FinancieraServer.ServiceImplementations;

var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

builder.Services.AddSingleton<SessionService>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<CatalogService>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<SessionService>();
    serviceBuilder.AddServiceEndpoint<SessionService, ISessionService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/Service.svc");
    serviceBuilder.AddService<AccountService>();
    serviceBuilder.AddServiceEndpoint<AccountService, IAccountService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/AccountService.svc");
    serviceBuilder.AddService<CatalogService>();
    serviceBuilder.AddServiceEndpoint<CatalogService, ICatalogService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/CatalogService.svc");

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
