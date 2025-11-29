using Firmeza.web.Data.Entity;
using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Firmeza.Tests.Validators
{
    public class ClienteValidationTests
    {
        [Fact]
        public void Cliente_ShouldBeValid_WhenAllFieldsAreCorrect()
        {
            // Arrange
            var cliente = new Cliente
            {
                NombreCompleto = "Juan Pérez",
                Documento = "12345678",
                Correo = "juan@example.com",
                Telefono = "3001234567",
                Direccion = "Calle 123",
                Activo = true,
                UserId = "user-id-123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cliente, new ValidationContext(cliente), validationResults, true);

            // Assert
            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Cliente_ShouldBeInvalid_WhenNombreCompletoIsEmpty()
        {
            // Arrange
            var cliente = new Cliente
            {
                NombreCompleto = "",
                Documento = "12345678",
                Correo = "juan@example.com",
                Telefono = "3001234567",
                Direccion = "Calle 123",
                Activo = true,
                UserId = "user-id-123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cliente, new ValidationContext(cliente), validationResults, true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().ContainSingle();
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("@example.com")]
        [InlineData("test@")]
        public void Cliente_ShouldBeInvalid_WhenEmailFormatIsWrong(string email)
        {
            // Arrange
            var cliente = new Cliente
            {
                NombreCompleto = "Juan Pérez",
                Documento = "12345678",
                Correo = email,
                Telefono = "3001234567",
                Direccion = "Calle 123",
                Activo = true,
                UserId = "user-id-123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cliente, new ValidationContext(cliente), validationResults, true);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Cliente_ShouldBeInvalid_WhenNombreContainsNumbers()
        {
            // Arrange
            var cliente = new Cliente
            {
                NombreCompleto = "Juan123",
                Documento = "12345678",
                Correo = "juan@example.com",
                Telefono = "3001234567",
                Direccion = "Calle 123",
                Activo = true,
                UserId = "user-id-123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cliente, new ValidationContext(cliente), validationResults, true);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}
