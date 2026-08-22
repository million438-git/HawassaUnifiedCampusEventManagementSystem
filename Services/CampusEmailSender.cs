using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HawassaUnifiedCampusEventManagementSystem.Services
{
    public class CampusEmailSender : IEmailSender
    {
        private readonly ILogger<CampusEmailSender> _logger;

        public CampusEmailSender(ILogger<CampusEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation("CampusEmailSender: Dispatching email notification to {Email} | Subject: '{Subject}'", email, subject);
            return Task.CompletedTask;
        }
    }
}
