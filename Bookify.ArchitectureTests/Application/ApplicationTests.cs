using Bookify.Application.Abstractions.Messaging;
using Bookify.ArchitectureTests.Infrastructure;
using FluentValidation;
using NetArchTest.Rules;
using Shouldly;

namespace Bookify.ArchitectureTests.Application;

public sealed class ApplicationTests : BaseTest
{
    [Fact]
    public void CommandHandler_Should_HaveNameEndingWithCommandHandler()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void CommandHandler_Should_NotBePublic()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .NotBePublic()
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void QueryHandler_Should_HaveNameEndingWithQueryHandler()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void QueryHandler_Should_NotBePublic()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .NotBePublic()
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Validator_Should_HaveNameEndingWithValidator()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Validator_Should_NotBePublic()
    {
        TestResult result = Types.InAssembly(ApplicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .NotBePublic()
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}