using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartBusiness.Api.Controllers;
using SmartBusiness.Application.Commands.Users.ChangePassword;
using SmartBusiness.Application.Commands.Users.Create;
using SmartBusiness.Application.Commands.Users.Delete;
using SmartBusiness.Application.Commands.Users.Update;
using SmartBusiness.Contracts.Requests.Users;

namespace SmartBusiness.Tests.Controllers.Users
{
    public class UserControllerSuccessScenarioTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _userController;

        public UserControllerSuccessScenarioTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userController = new UserController(_mediatorMock.Object);
        }


        [Fact]
        public async Task Create_WhenSuccessful_ReturnsCreatedResult()
        {
            // Arrange
            var request = new CreateRequest("John", "john@gmail.com", "!Qwerty123");
            const string expectedResponse = "John";

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userController.Create(request);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }


        [Fact]
        public async Task Update_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var request = new UpdateRequest(Guid.NewGuid(), "john@gmail.com", "!Qwerty123");
            const string expectedResponse = "John";

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userController.Update(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ChangePassword_WhenSuccessful_ReturnsOkResult()
        {
            // Arrange
            var request = new ChangePasswordRequest(Guid.NewGuid(), "oldPassword", "newPassword");
            const string expectedResponse = "Password changed successfully";

            _mediatorMock.Setup(m => m.Send(It.IsAny<ChangeUserPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _userController.ChangePassword(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Delete_WhenSuccessful_ReturnsNoContentResult()
        {
            // Arrange
            var request = new DeleteRequest(Guid.NewGuid());

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userController.Delete(request);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}