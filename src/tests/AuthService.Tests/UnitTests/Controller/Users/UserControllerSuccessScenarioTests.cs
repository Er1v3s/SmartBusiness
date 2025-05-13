using Moq;
using MediatR;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using AuthService.Api.Controllers;
using AuthService.Application.Commands.Account;
using AuthService.Contracts.Requests.Account;
using AuthService.Application.Commands.Auth;
using AuthService.Contracts.Requests.Auth;

namespace AuthService.Tests.UnitTests.Controller.Users
{
    public class UserControllerSuccessScenarioTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AccountController _accountController;
        private readonly AuthController _authController;

        public UserControllerSuccessScenarioTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _accountController = new AccountController(_mediatorMock.Object);
            _authController = new AuthController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_WhenSuccessful_ReturnsCreatedResult()
        {
            // Arrange
            var request = new RegisterUserRequest("John", "john@gmail.com", "!Qwerty123");
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()));

            // Act
            var result = await _authController.RegisterUser(request);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }


        [Fact]
        public async Task Update_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateUserRequest("john@gmail.com", "!Qwerty123");
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()));

            // Act
            var result = await _accountController.UpdateUser(Guid.NewGuid(), request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ChangePassword_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var request = new ChangePasswordRequest("oldPassword", "newPassword");
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<ChangeUserPasswordCommand>(), It.IsAny<CancellationToken>()));

            // Act
            var result = await _accountController.ChangePassword(Guid.NewGuid(), request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Delete_WhenSuccessful_ReturnsNoContentResult()
        {
            // Arrange
            var request = new DeleteUserRequest(Guid.NewGuid());

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _accountController.DeleteUser(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}