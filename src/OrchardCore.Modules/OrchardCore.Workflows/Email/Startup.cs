using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Modules;
using OrchardCore.Workflows.Email.Activities;
using OrchardCore.Workflows.Email.Drivers;
using OrchardCore.Workflows.Helpers;

namespace OrchardCore.Workflows.Email
{
    [Feature("OrchardCore.Workflows.Email")]
    [RequireFeatures("OrchardCore.Liquid")]
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SmtpOptions>(_configuration.GetSection("Modules:OrchardCore.Email"));
            services.AddActivity<EmailTask, EmailTaskDisplay>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }
    }
}
