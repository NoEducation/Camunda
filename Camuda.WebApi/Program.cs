using Camunda.WebApi.Infrastructure.BackgroundServices;
using Camunda.WebApi.Infrastructure.HostedServices;
using Camunda.WebApi.Infrastructure.Services.Email;
using Camunda.WebApi.Infrastructure.Services.Http;
using Camunda.WebApi.Infrastructure.Services.ZeebeEngine;
using Camunda.WebApi.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
builder.Services.AddHttpClient();

builder.Services.Configure<EmailOptions>(options =>
    builder.Configuration.GetSection(EmailOptions.Key).Bind(options));

builder.Services.Configure<CamundaOptions>(options =>
    builder.Configuration.GetSection(CamundaOptions.Key).Bind(options));

builder.Services.Configure<CamundaEnvironmentOptions>(options =>
    builder.Configuration.GetSection(CamundaEnvironmentOptions.Key).Bind(options));

builder.Services.Configure<CamundaConsoleOptions>(options =>
    builder.Configuration.GetSection(CamundaConsoleOptions.Key).Bind(options));

//TODO.DA It's should be registered as singleton switching to scoped causes error in working with ZeebeClientService
builder.Services.AddSingleton<IZeebeClientService, ZeebeClientService>();

builder.Services.AddScoped<ICamundaHttpService, CamundaHttpService>();
builder.Services.AddScoped<ICamoundaOperateService, CamoundaOperateService>();

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<SmtpClientWrapper>();

builder.Services.AddHostedService<DeployResourcesHostedService>();
builder.Services.AddHostedService<GetTimeJobBackgroundService>();
builder.Services.AddHostedService<CreateMakeGreetingBackgroundService>();
builder.Services.AddHostedService<SendEmailBackgroundService>();
builder.Services.AddHostedService<EmailWasSendEventBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();