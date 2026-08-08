namespace Hotel.Domain.Customers;

public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string DocumentNumber { get; private set; }
    public CustomerLocation Location { get; private set; }

    public Customer() { }

    private Customer(
        Guid id,
        string firstName,
        string lastName,
        string phone,
        string email,
        string documentNumber,
        CustomerLocation location)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        DocumentNumber = documentNumber;
        Location = location;
    }

    public static Customer Create(
        string firstName,
        string lastName,
        string phone,
        string email,
        string documentNumber,
        CustomerLocation location)
    {
        return new Customer(
            Guid.NewGuid(),
            firstName,
            lastName,
            phone,
            email,
            documentNumber,
            location);
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        string phone,
        string email,
        string documentNumber,
        CustomerLocation location)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        DocumentNumber = documentNumber;
        Location = location;
    }
}