using FluentAssertions;
using MediatR;
using Moq;
using SmartBusiness.Api.Controllers;
using SmartBusiness.Application.Commands.Users.ChangePassword;
using SmartBusiness.Application.Commands.Users.Create;
using SmartBusiness.Application.Commands.Users.Delete;
using SmartBusiness.Application.Commands.Users.Update;
using SmartBusiness.Contracts.Exceptions.Users;
using SmartBusiness.Contracts.Requests.Users;

namespace SmartBusiness.Tests.Controllers.Users
{
    public class UserControllerExceptionScenarioTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _userController;

        public UserControllerExceptionScenarioTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userController = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_WhenUserWithThisEmailExist_ThrowsUserAlreadyExistException()
        {
            // Arrange
            var request = new CreateRequest("John", "john@gmail.com", "!Qwerty123");

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserAlreadyExistsException());

            // Act
            Func<Task> act = async () => await _userController.Create(request);

            // Assert
            await act.Should().ThrowAsync<UserAlreadyExistsException>();
        }

        [Fact]
        public async Task Update_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            var request = new UpdateRequest(Guid.NewGuid(), "John", "john@gmail.com");

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            // Act
            Func<Task> act = async () => await _userController.Update(request);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Fact]
        public async Task ChangePassword_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            var request = new ChangePasswordRequest(Guid.NewGuid(), "!Qwerty123", "321ytrewQ!");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ChangeUserPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            // Act
            Func<Task> act = async () => await _userController.ChangePassword(request);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }

        [Fact]
        public async Task ChangePassword_WhenUserPassWrongOldPassword_ThrowsInvalidPasswordException()
        {
            // Arrange
            var request = new ChangePasswordRequest(Guid.NewGuid(), "!Password123", "!Qwerty123");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ChangeUserPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidPasswordException("Incorrect current password"));

            // Act
            Func<Task> act = async () => await _userController.ChangePassword(request);

            // Assert
            await act.Should().ThrowAsync<InvalidPasswordException>();
        }

        [Fact]
        public async Task ChangePassword_WhenUserPassTheSameOldAndNewPassword_ThrowsInvalidPasswordException()
        {
            // Arrange
            var request = new ChangePasswordRequest(Guid.NewGuid(), "!Qwerty123", "!Qwerty123");
            var expectedMessage = "New password cannot be the same as the old password";

            _mediatorMock.Setup(m => m.Send(It.IsAny<ChangeUserPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidPasswordException(expectedMessage));

            // Act
            Func<Task> act = async () => await _userController.ChangePassword(request);

            // Assert
            await act.Should().ThrowAsync<InvalidPasswordException>().WithMessage("New password cannot be the same as the old password");
        }

        [Fact]
        public async Task Delete_WhenUserDoesNotExist_ThrowsUserNotFoundException()
        {
            // Arrange
            var request = new DeleteRequest(Guid.NewGuid());

            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException());

            // Act
            Func<Task> act = async () => await _userController.Delete(request);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }
    }
}