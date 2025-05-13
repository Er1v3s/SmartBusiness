using Moq;
using MediatR;
using FluentAssertions;
using AuthService.Api.Controllers;
using AuthService.Application.Commands.Account;
using AuthService.Contracts.Exceptions.Users;
using AuthService.Contracts.Requests.Account;
using AuthService.Application.Commands.Auth;
using AuthService.Contracts.Requests.Auth;

namespace AuthService.Tests.UnitTests.Controller.Users
{
    public class UserControllerExceptionScenarioTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AccountController _accountController;
        private readonly AuthController _authController;

        public UserControllerExceptionScenarioTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _accountController = new AccountController(_mediatorMock.Object);
            _authController = new AuthController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_WhenUserWithThisEmailExist_ThrowsUserAlreadyExistException()
        {
            // Arrange
            var request = new RegisterUserRequest("John", "john@gmail.com", "!Qwerty123");

            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserAlreadyExistsException());

            // Act
            Func<Task> act = async () => await _authController.RegisterUser(request);

            // Assert
            await act.Should().ThrowAsync<UserAlreadyExistsException>();
        }

        [Fact]
        public async Task Update_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest("John", "john@gmail.com");

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            // Act
            Func<Task> act = async () => await _accountController.UpdateUser(userId, request);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Fact]
        public async Task ChangePassword_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new ChangePasswordRequest("!Qwerty123", "321ytrewQ!");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ChangeUserPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            // Act
            Func<Task> act = async () => await _accountController.ChangePassword(userId, request);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Fact]
        public async Task ChangePassword_WhenUserPassWrongOldPassword_ThrowsInvalidPasswordException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new ChangePasswordRequest("!Password123", "!Qwerty123");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ChangeUserPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidPasswordException("Incorrect current password"));

            // Act
            Func<Task> act = async () => await _accountController.ChangePassword(userId, request);

            // Assert
            await act.Should().ThrowAsync<InvalidPasswordException>();
        }

        [Fact]
        public async Task ChangePassword_WhenUserPassTheSameOldAndNewPassword_ThrowsInvalidPasswordException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new ChangePasswordRequest("!Qwerty123", "!Qwerty123");
            var expectedMessage = "New password cannot be the same as the old password";

            _mediatorMock.Setup(m => m.Send(It.IsAny<ChangeUserPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidPasswordException(expectedMessage));

            // Act
            Func<Task> act = async () => await _accountController.ChangePassword(userId, request);

            // Assert
            await act.Should().ThrowAsync<InvalidPasswordException>().WithMessage("New password cannot be the same as the old password");
        }

        [Fact]
        public async Task Delete_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new DeleteUserRequest(userId);

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            // Act
            Func<Task> act = async () => await _accountController.DeleteUser(userId);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }
    }
}