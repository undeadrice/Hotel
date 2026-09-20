using FluentAssertions;
using Hotel.API.Auth;
using Hotel.Application.Pipeline;
using Hotel.Domain.Rooming.Entities;
using Hotel.Infrastructure;
using Hotel.Persistence;
using NetArchTest.Rules;
using Xunit;

namespace Hotel.ArchitectureTests
{
    public class DependencyTests
    {
        [Fact]
        public void Domain_Should_Not_Reference_Other_Projects()
        {
            var result = Types.InAssembly(typeof(Room).Assembly)
                .Should()
                .NotHaveDependencyOn("Hotel.Application")
                .And()
                .NotHaveDependencyOn("Hotel.Persistence")
                .And()
                .NotHaveDependencyOn("Hotel.Infrastructure")
                .And()
                .NotHaveDependencyOn("Hotel.API")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(BuildFailureMessage(result));
        }

        [Fact]
        public void Application_Should_Not_Reference_Other_Projects()
        {
            var result = Types.InAssembly(typeof(ICommand).Assembly)
                .Should()
                .NotHaveDependencyOn("Hotel.Persistence")
                .And()
                .NotHaveDependencyOn("Hotel.Infrastructure")
                .And()
                .NotHaveDependencyOn("Hotel.API")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(BuildFailureMessage(result));
        }

        [Fact]
        public void Persistence_Should_Not_Reference_Other_Projects()
        {
            var result = Types.InAssembly(typeof(PersistenceDbContext).Assembly)
                .Should()
                .NotHaveDependencyOn("Hotel.Infrastructure")
                .And()
                .NotHaveDependencyOn("Hotel.API")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(BuildFailureMessage(result));
        }

        [Fact]
        public void Infrastructure_Should_Not_Reference_Other_Projects()
        {
            var result = Types.InAssembly(typeof(InfraIdentityDbContext).Assembly)
                .Should()
                .NotHaveDependencyOn("Hotel.Persistence")
                .And()
                .NotHaveDependencyOn("Hotel.API")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(BuildFailureMessage(result));
        }

        [Fact]
        public void Api_Should_Reference_All_Projects()
        {
            var referencedAssemblies = typeof(AuthController).Assembly
                .GetReferencedAssemblies()
                .Select(a => a.Name)
                .ToHashSet();

            referencedAssemblies.Should().Contain(
            [
                "Hotel.Domain",
                "Hotel.Application",
                "Hotel.Persistence",
                "Hotel.Infrastructure"
            ]);
        }

        private static string BuildFailureMessage(TestResult result)
        {
            var failingTypes = result.FailingTypeNames ?? Enumerable.Empty<string>();
            return $"The following types violate the dependency rule:{Environment.NewLine}" +
                   string.Join(Environment.NewLine, failingTypes);
        }
    }
}
