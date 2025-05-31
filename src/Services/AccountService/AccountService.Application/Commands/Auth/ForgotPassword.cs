using System.Web;
using AccountService.Application.Abstracts;
using AccountService.Contracts.Exceptions.Users;
using FluentValidation;
using MediatR;

namespace AccountService.Application.Commands.Auth
{
    public record ForgotPasswordCommand(string Email) : IRequest;

    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>;

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IResetPasswordTokenProcessor _resetPasswordTokenProcessor;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService, IResetPasswordTokenProcessor resetPasswordTokenProcessor)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _resetPasswordTokenProcessor = resetPasswordTokenProcessor;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var query = _userRepository.GetQueryableIncludingProperties();
            query = query.Where(u => u.Email == request.Email);

            var user = await _userRepository.GetFilteredUserAsync(query, cancellationToken)
                       ?? throw new UserNotFoundException();

            var resetToken = _resetPasswordTokenProcessor.GenerateResetPasswordToken();
            var resetUrl = $"http://localhost:5000/reset-password?token={HttpUtility.UrlEncode(resetToken)}";

            user.ResetPasswordToken = resetToken;
            user.ResetPasswordTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(15);

            await _userRepository.UpdateUserAsync(user);

            var httpContent = $"<p>To reset your password click this link below: </p><p><a href=\"{resetUrl}\">Reset Password</a></p>" +
                              "<p>If you did not request a password reset, please ignore this email.</p>" +
                              "<p>Thank you!</p>";

            await _emailService.SendAsync(user.Email, "Password reset", httpContent);
        }
    }
}
