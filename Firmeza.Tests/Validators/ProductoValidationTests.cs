using Firmeza.web.Data.Entity;
using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Firmeza.Tests.Validators
{
    public class ProductoValidationTests
    {
        [Fact]
        public void Producto_ShouldBeValid_WhenAllFieldsAreCorrect()
        {
            // Arrange
            var producto = new Producto
            {
                Nombre = "Test Product",
                Descripcion = "Test Description",
                PrecioUnitario = 100.50m,
                Categoria = "Electronics"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(producto, new ValidationContext(producto), validationResults, true);

            // Assert
            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Producto_ShouldBeInvalid_WhenNombreIsEmpty()
        {
            // Arrange
            var producto = new Producto
            {
                Nombre = "",
                Descripcion = "Test Description",
                PrecioUnitario = 100.50m,
                Categoria = "Electronics"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(producto, new ValidationContext(producto), validationResults, true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().ContainSingle();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Producto_ShouldBeInvalid_WhenPrecioIsNegativeOrZero(decimal precio)
        {
            // Arrange
            var producto = new Producto
            {
                Nombre = "Test Product",
                Descripcion = "Test Description",
                PrecioUnitario = precio,
                Categoria = "Electronics"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(producto, new ValidationContext(producto), validationResults, true);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}
