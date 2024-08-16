using ErrorOr;
using Throw;

namespace Domain.Users;
public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    private User(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public static ErrorOr<User> Create(int id, string name, string email)
    {
        //TODO: feels sus
        name.ThrowIfNull().IfEmpty();
        email.ThrowIfNull().IfEmpty();

        if (!email.Contains('@'))
            return Error.Validation("Email must contain '@' symbol");

        return new User(id, name, email);
    }
}

