using System.Security.Cryptography;

var folder = @"C:\Users\bruce\Desktop\ssltest";
Directory.CreateDirectory(folder);

using var rsa = RSA.Create(2048);
File.WriteAllText(Path.Combine(folder, "private.pem"), rsa.ExportRSAPrivateKeyPem());
File.WriteAllText(Path.Combine(folder, "public.pem"), rsa.ExportRSAPublicKeyPem());

Console.WriteLine($"Keys written to {folder}");