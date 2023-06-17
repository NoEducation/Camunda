using System.Text;
using Camunda.WebApi.Infrastructure.BackgroundServices;
using Camunda.WebApi.Infrastructure.HostedServices;
using Camunda.WebApi.Infrastructure.Services;
using Camunda.WebApi.Options;
using dotenv.net.DependencyInjection.Microsoft;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEnv(builderEnv =>
{
    builderEnv
        .AddEnvFile(builder.Configuration
            .GetValue<bool>("Comunda:IsLocalConnection")
            ? "CamundaLocal.env"
            : "CamundaCloud.env")
        .AddThrowOnError(false)
        .AddEncoding(Encoding.ASCII);
});

builder.Services.AddEnvReader();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();

builder.Services.Configure<EmailOptions>(options =>
    builder.Configuration.GetSection(EmailOptions.Key).Bind(options));

builder.Services.Configure<CamundaOptions>(options =>
    builder.Configuration.GetSection(CamundaOptions.Key).Bind(options));

builder.Services.AddSingleton<IZeebeClientService, ZeebeClientService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<SmtpClientWrapper>();

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