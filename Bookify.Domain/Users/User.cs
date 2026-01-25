using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Events;

namespace Bookify.Domain.Users;

internal sealed class User : Entity
{
    internal FirstName FirstName { get; private set; }

    internal LastName LastName { get; private set; }

    internal Email Email { get; private set; }

    private User(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static User Create(FirstName firstName, LastName lastName, Email email)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }
}