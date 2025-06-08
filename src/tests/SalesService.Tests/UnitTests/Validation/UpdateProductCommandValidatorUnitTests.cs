//using FluentValidation.TestHelper;
//using NanoidDotNet;
//using SalesService.Application.Commands.Products;

//namespace SalesService.Tests.UnitTests.Validation
//{
//    public class UpdateProductCommandValidatorUnitTests
//    {
//        private readonly UpdateProductCommandValidator _validator;

//        public UpdateProductCommandValidatorUnitTests()
//        {
//            _validator = new UpdateProductCommandValidator();
//        }
        
//        public static IEnumerable<object[]> InvalidProductName()
//        {
//            yield return [new string('a', 101)]; // max 100 characters
//        }

//        [Theory]
//        [InlineData(null)] // not null
//        [InlineData("")] // not empty
//        [InlineData("ab")] // min 3 characters
//        [MemberData(nameof(InvalidProductName))] // max 100 characters
//        public void Validate_ForInvalidProductName_ShouldReturnValidationError(string invalidProductName)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), invalidProductName, "testDescription", 
//                new List<string> { "testCategory" }, 99.99m, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldHaveValidationErrorFor(r => r.Name);
//        }

//        [Theory]
//        [InlineData("TestProductName")] 
//        [InlineData("Test Product Name")]
//        [InlineData("Test ProductName 123")]
//        [InlineData("Test ProductName 123 !@#$%^&*()")]
//        public void Validate_ForValidProductName_ShouldNotReturnValidationError(string validProductName)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), validProductName, "testDescription", 
//                new List<string> { "testCategory" }, 99.99m, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldNotHaveValidationErrorFor(r => r.Name);
//        }

//        public static IEnumerable<object[]> InvalidProductDescription()
//        {
//            yield return [$"{new string('a', 501)}"]; // invalid description (too long)
//        }

//        [Theory]
//        [InlineData(null)] // not null
//        [InlineData("")] // not empty
//        [InlineData("ab")] // min 3 characters
//        [MemberData(nameof(InvalidProductDescription))] // max 500 characters
//        public void Validate_ForInvalidProductDescription_ShouldReturnValidationError(string invalidProductDescription)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), "TestProductName", invalidProductDescription, 
//                new List<string> { "testCategory" }, 99.99m, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldHaveValidationErrorFor(r => r.Description);
//        }

//        public static IEnumerable<object[]> ValidProductDescription()
//        {
//            yield return [$"{new string('a', 499)}"]; // invalid description (too long)
//        }

//        [Theory]
//        [InlineData("TestProductDescription")]
//        [InlineData("Test Product Description")]
//        [InlineData("Test ProductDescription 123")]
//        [InlineData("Test ProductDescription 123 !@#$%^&*()")]
//        [MemberData(nameof(ValidProductDescription))]
//        public void Validate_ForValidProductDescription_ShouldNotReturnValidationError(string validProductDescription)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), "TestProductName", validProductDescription, 
//                new List<string> { "testCategory" }, 99.99m, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldNotHaveValidationErrorFor(r => r.Description);
//        }

//        public static IEnumerable<object[]> InvalidProductCategory()
//        {
//            yield return [$"{new string('a', 51)}"]; // invalid description (too long)
//        }

//        [Theory]
//        [InlineData(null)] // not null
//        [InlineData("")] // not empty
//        [InlineData("ab")] // min 3 characters
//        [MemberData(nameof(InvalidProductCategory))] // max 50 characters
//        public void Validate_ForInvalidProductCategory_ShouldReturnValidationError(string invalidProductCategory)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), "TestProductName", "TestProductDescription", 
//                new List<string> { invalidProductCategory }, 99.99m, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldHaveValidationErrorFor(r => r.Category);
//        }

//        public static IEnumerable<object[]> ValidProductCategory()
//        {
//            yield return [$"{new string('a', 50)}"]; // invalid description (too long)
//        }

//        [Theory]
//        [InlineData("TestProductCategory")]
//        [InlineData("Test Product Category")]
//        [InlineData("Test ProductCategory 123")]
//        [InlineData("Test ProductCategory 123 !@#$%^&*()")]
//        [MemberData(nameof(ValidProductCategory))]
//        public void Validate_ForValidProductCategory_ShouldNotReturnValidationError(string validProductCategory)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), "TestProductName", "TestProductDescription", 
//                new List<string> { validProductCategory }, 99.99m, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldNotHaveValidationErrorFor(r => r.Category);
//        }

//        [Theory]
//        [InlineData(null)] // not null
//        [InlineData(0)] // greater than 0
//        [InlineData(35.001)] // up to 2 decimal places
//        [InlineData(10000000)] // lest than 100k
//        public void Validate_ForInvalidProductPrice_ShouldReturnValidationError(decimal price)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), "TestProductName", "TestProductDescription", 
//                new List<string> { "testCategory" }, price, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldHaveValidationErrorFor(r => r.Price);
//        }

//        [Theory]
//        [InlineData(0.01)]
//        [InlineData(50)]
//        [InlineData(50.00)]
//        [InlineData(99.99)]
//        [InlineData(99999.99)]
//        public void Validate_ForValidProductPrice_ShouldNotReturnValidationError(decimal price)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), "TestProductName", "TestProductDescription", 
//                new List<string> { "testCategory" }, price, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldNotHaveValidationErrorFor(r => r.Price);
//        }

//        [Theory]
//        [InlineData(null)] // not null
//        [InlineData(0)] // greater than 0
//        [InlineData(101)] // lest than or equal to 100
//        public void Validate_ForInvalidProductTax_ShouldReturnValidationError(int tax)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), "TestProductName", "TestProductDescription", 
//                new List<string> { "testCategory" }, 99.99m, tax, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldHaveValidationErrorFor(r => r.Tax);
//        }

//        [Theory]
//        [InlineData(1)]
//        [InlineData(50)]
//        [InlineData(100)]
//        public void Validate_ForValidProductTax_ShouldNotReturnValidationError(int tax)
//        {
//            // Arrange
//            var command = new UpdateProductCommand(Nanoid.Generate(size: 17), "TestProductName", "TestProductDescription", 
//                new List<string> { "testCategory" }, tax, 5, "testImageFile");

//            // Act
//            var result = _validator.TestValidate(command);

//            // Assert
//            result.ShouldNotHaveValidationErrorFor(r => r.Tax);
//        }
//    }
//}