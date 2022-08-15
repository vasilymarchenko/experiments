using FluentAssertions;
using FluentValidation;
using Xunit;

namespace UnitTests.Validation
{
    public class MyModel
    {
        public string? ModelName { get; set; }
    }

    public class MyModelValidator: AbstractValidator<MyModel>
    {
        public MyModelValidator()
        {
            // CascadeMode doesn't works.
            // Either in cascade notation either in sequence 
            RuleFor(x => x.ModelName).Cascade(CascadeMode.Stop).NotNull().NotEmpty();
                
            //It was expected the next steps will not check if CascadeMode is stop, but not...

                //.When(x => x.ModelName.Length > 5).WithMessage("Name must be greater than 5")
                //.When(x => x.ModelName.Substring(1).Length > 0).WithMessage("Substring must works ))");
            RuleFor(x => x.ModelName).Must(s => s.Length > 5).WithMessage("Name must be greater than 5")
                .When(x => x.ModelName.Substring(1).Length > 0).WithMessage("Substring must works ))");
        }
    }
    
    public class FluentValidatorTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void Test1(string name, bool result)
        {
            var model = new MyModel()
            {
                ModelName = name
            };

            var validator = new MyModelValidator();
            var validationResult = validator.Validate(model);
            validationResult.IsValid.Should().Be(result);
        }
    }
}
