using System.Reflection;
using Bookify.ArchitectureTests.Infrastructure;
using Bookify.Domain.Abstractions;
using NetArchTest.Rules;
using Shouldly;

namespace Bookify.ArchitectureTests.Domain;

public sealed class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void DomainEvent_Should_HaveDomainEventPostfix()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (Type entityType in entityTypes)
        {
            ConstructorInfo[] constructors = entityType
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            if (!constructors.Any(constructor => constructor.IsPrivate && constructor.GetParameters().Length == 0))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.ShouldBeEmpty();
    }
}