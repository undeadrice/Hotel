using FluentAssertions;
using Hotel.Application.Pipeline;
using MediatR;
using NetArchTest.Rules;
using Xunit;

namespace Hotel.ArchitectureTests
{
    public class HandlersTests
    {
        [Fact]
        public void Handlers_Should_BeInternal()
        {
            var result = Types.InAssembly(typeof(ICommand).Assembly)
                .That()
                .ImplementInterface(typeof(IRequestHandler<>))
                .Or()
                .ImplementInterface(typeof(IRequestHandler<,>))
                .Should()
                .NotBePublic()
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                BuildFailureMessage(result));
        }

        private static string BuildFailureMessage(TestResult result)
        {
            var failingTypes = result.FailingTypes?.Select(t => t.FullName) ?? Enumerable.Empty<string>();
            return $"The following types should be internal but are not:{Environment.NewLine}" +
                   string.Join(Environment.NewLine, failingTypes);
        }
    }
}
