using Bookify.ArchitectureTests.Infrastructure;
using NetArchTest.Rules;
using Shouldly;

namespace Bookify.ArchitectureTests.Layers;

public sealed class LayerTests : BaseTest
{
    [Fact]
    public void DomainLayer_Should_NotHaveDependencyOnApplicationLayer()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void DomainLayer_Should_NotHaveDependencyOnInfrastructureLayer()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void DomainLayer_Should_NotHaveDependencyOnPresentationLayer()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}