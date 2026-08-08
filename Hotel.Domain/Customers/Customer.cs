namespace Hotel.Domain.Customers;

public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string DocumentNumber { get; private set; }

    public Customer() { }

    private Customer(
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

    public static Customer Create(
        string firstName,
        string lastName,
        string phone,
        string email,
        string documentNumber)
    {
        return new Customer(
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