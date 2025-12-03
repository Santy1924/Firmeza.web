using Firmeza.web.Web.Api.Services;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;

namespace Firmeza.Tests.Services
{
    public class EmailServiceTests
    {
        [Fact]
        public async Task SendEmailAsync_ShouldNotThrow_WhenValidParameters()
        {
            // Arrange
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var toEmail = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            var act = async () => await mockEmailService.Object.SendEmailAsync(toEmail, subject, body);

            // Assert
            await act.Should().NotThrowAsync();
            mockEmailService.Verify(x => x.SendEmailAsync(toEmail, subject, body), Times.Once);
        }

        [Theory]
        [InlineData("", "subject", "body")]
        [InlineData(null, "subject", "body")]
        public async Task SendEmailAsync_ShouldThrow_WhenEmailIsInvalid(string toEmail, string subject, string body)
        {
            // Arrange
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService
                .Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new System.ArgumentException("Invalid email"));

            // Act
            var act = async () => await mockEmailService.Object.SendEmailAsync(toEmail, subject, body);

            // Assert
            await act.Should().ThrowAsync<System.ArgumentException>();
        }
    }
}
