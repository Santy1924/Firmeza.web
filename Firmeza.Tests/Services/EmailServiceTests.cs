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
            var emailService = new EmailService();
            var toEmail = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            var act = async () => await emailService.SendEmailAsync(toEmail, subject, body);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Theory]
        [InlineData("", "subject", "body")]
        [InlineData(null, "subject", "body")]
        public async Task SendEmailAsync_ShouldThrow_WhenEmailIsInvalid(string toEmail, string subject, string body)
        {
            // Arrange
            var emailService = new EmailService();

            // Act
            var act = async () => await emailService.SendEmailAsync(toEmail, subject, body);

            // Assert
            await act.Should().ThrowAsync<System.ArgumentException>();
        }
    }
}
