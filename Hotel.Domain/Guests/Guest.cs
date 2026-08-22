using Hotel.Domain.Guests.Exceptions;

namespace Hotel.Domain.Guests;

public class Guest
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string DocumentNumber { get; private set; }

#pragma warning disable CS8618
    internal Guest() { }
#pragma warning restore CS8618

    private Guest(
        Guid id,
        string firstName,
        string lastName,
        string phone,
        string email,
        string documentNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        DocumentNumber = documentNumber;
    }

    public static Guest Create(
        string firstName,
        string lastName,
        string phone,
        string email,
        string documentNumber)
    {
        Validate(firstName, lastName, phone, email, documentNumber);

        return new Guest(
            Guid.NewGuid(),
            firstName,
            lastName,
            phone,
            email,
            documentNumber);
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        string phone,
        string email,
        string documentNumber)
    {
        Validate(firstName, lastName, phone, email, documentNumber);

        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        DocumentNumber = documentNumber;
    }

    private static void Validate(
        string firstName,
        string lastName,
        string phone,
        string email,
        string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new GuestFirstNameRequiredException();
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new GuestLastNameRequiredException();
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new GuestPhoneRequiredException();
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new GuestEmailRequiredException();
        }

        if (string.IsNullOrWhiteSpace(documentNumber))
        {
            throw new GuestDocumentNumberRequiredException();
        }
    }
}
