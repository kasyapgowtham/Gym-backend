using backend.Services.Interfaces;
using Resend;

namespace backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        public EmailService(IResend resend) => _resend = resend;

        public async Task SendWelcomeMail(string email)
        {
            var message = new EmailMessage
            {
                From = "gowthamghantasala123@gmail.com", // Use your verified domain later
                To = email,
                Subject = "Registration Successful!",
                HtmlBody = "<strong>Welcome! Your registration is complete.</strong>"
            };

            await _resend.EmailSendAsync(message);
        }
    }
}
