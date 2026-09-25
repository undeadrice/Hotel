namespace Hotel.Auth.Infrastructure.Secrets;

public record DbConfig(
    string Host,
    int Port,
    string Dbname,
    string Username,
    string Password
);