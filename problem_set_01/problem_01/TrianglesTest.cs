using System;
using Xunit;
using Triangles;

namespace TriangleTests
{
    public class TriangleTests
    {
        [Fact]
        public void Area_ReturnsCorrectValue_EquilateralTriangle()
        {

            // Arrange
            double a = 6;
            double expected = 15.588457268119896;
            EquilateralTriangle triangle = new EquilateralTriangle(a);

            // Act
            double actual = triangle.Area();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Area_ReturnsCorrectValue_IsoscelesTriangle()
        {

            // Arrange
            double a = 6;
            double b = 4;
            double expected = 7.9372539331937721;
            IsoscelesTriangle triangle = new IsoscelesTriangle(a, b);

            // Act
            double actual = triangle.Area();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Area_ReturnsCorrectValue_RightTriangle()
        {

            // Arrange
            double a = 5;
            double b = 12;
            double expected = 30;
            RightTriangle triangle = new RightTriangle(a, b);

            // Act
            double actual = triangle.Area();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Perimeter_ReturnsCorrectValue_EquilateralTriangle()
        {

            // Arrange
            double a = 6;
            double expected = 18;
            EquilateralTriangle triangle = new EquilateralTriangle(a);

            // Act
            double actual = triangle.Perimeter();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Perimeter_ReturnsCorrectValue_IsoscelesTriangle()
        {

            // Arrange
            double a = 6;
            double b = 4;
            double expected = 14;
            IsoscelesTriangle triangle = new IsoscelesTriangle(a, b);

            // Act
            double actual = triangle.Perimeter();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Perimeter_ReturnsCorrectValue_RightTriangle()
        {

            // Arrange
            double a = 5;
            double b = 12;
            double expected = 30;
            RightTriangle triangle = new RightTriangle(a, b);

            // Act
            double actual = triangle.Perimeter();

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}