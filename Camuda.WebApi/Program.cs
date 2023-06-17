using Camuda.WebApi.Infrastructure.BackgroundServices;
using Camuda.WebApi.Infrastructure.HostedServices;
using Camuda.WebApi.Infrastructure.Services;
using Camuda.WebApi.Options;
using dotenv.net.DependencyInjection.Microsoft;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEnv(builderEnv => {
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

builder.Services.Configure<ComundaOptions>(options =>
    builder.Configuration.GetSection(ComundaOptions.Key).Bind(options));

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
