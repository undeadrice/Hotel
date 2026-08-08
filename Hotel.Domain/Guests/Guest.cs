namespace Hotel.Domain.Guests;

public class Guest
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string DocumentNumber { get; private set; }

    public Guest() { }

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
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        DocumentNumber = documentNumber;
    }
}