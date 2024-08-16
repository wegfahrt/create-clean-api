using ErrorOr;
using FluentAssertions;
using Moq;

using Application.Interfaces;
using Application.Users.Queries;
using Domain.Users;

namespace Tests;
public class GetUserQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        //TODO: learn Moq lib
        var mockRepo = new Mock<IUserRepository>();
        var expectedUser = User.Create(1, "John Doe", "john@example.com").Value;
        mockRepo.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(expectedUser);

        var handler = new GetUserQueryHandler(mockRepo.Object);
        var query = new GetUserQuery(1);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(Error.NotFound("User not found"));

        var handler = new GetUserQueryHandler(mockRepo.Object);
        var query = new GetUserQuery(1);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }
}

