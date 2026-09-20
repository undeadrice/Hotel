# Hotel PMS

A Property Management System (PMS) for hotels built with **.NET 10**, following **Clean Architecture**, **Domain-Driven Design**, and **CQRS**. The system covers core operations of a front-desk/back-office hotel application: managing rooms and rate plans, creating reservations, checking guests in and out, posting charges and payments through folio based fiscal accounting subsystem and running an end-of-day routine that advances the hotel's bsiness date and automatically posts overnight room charges.

The system is thoroughly tested with both unit and integration tests.

The frontend is implemented as a separate Angular application and is intended to be moved to its own private repository. As it does not follow the best practices.

---

## What the system does

- **Authentication & role-based authorization** – JWT authentication via ASP.NET Core Identity, with roles and fine-grained permissions enforced as MediatR pipeline behaviors.
- **Rooming** – room types and individual rooms, with activation/deactivation.
- **Rate plans** – priced per room type, with active date ranges and linked transaction codes.
- **Guest management** – guest profiles with contact and identity document data.
- **Reservations** – guest-based reservations with a full lifecycle: `Reserved → DueIn → InHouse → CheckedOut` (plus `NoShow`), with room-availability validation on creation.
- **Fiscal accounting** – each reservation automatically opens a *fiscal account* containing *folios* (billing buckets). Folios hold *folio items* (charges and payments - basically a transaction), can be settled when balanced, and the account is checked out once fully settled.
- **Transactions** – configurable transaction groups and transaction codes (e.g. Stay, Food & Beverage, Payment) used to classify charges and payments.
- **Number cycles** – configurable, prefix-based identifier generation (e.g. `RES-1`, `FA-1`) for reservation and fiscal-account numbering.
- **End-of-day processing** – advances the hotel business date, transitions reservation states, and automatically posts room charges for in-house guests.
- **Dashboard** – read-only KPIs: room count, occupied rooms, guest count, guests on site, occupancy percentage, and current business date.
- **Server configuration** – a single system configuration holding the hotel time zone, current business date, and seed state.

---

## Solution structure

| Project | Description |
| --- | --- |
| `Hotel.Domain` | Core business entities, aggregates, domain services, enums, and business rules. No external dependencies. |
| `Hotel.Application` | CQRS use cases (commands/queries) implemented with **MediatR**, FluentValidation validators, DTOs, and cross-cutting pipeline behaviors. |
| `Hotel.Persistence` | EF Core (`PersistenceDbContext`) data access for the business domain, repositories, read repositories, and unit-of-work implementation (also uses **Dapper** for some read paths). |
| `Hotel.Infrastructure` | ASP.NET Core Identity (`InfraIdentityDbContext`), JWT authentication, and implementations for auth, user, role, and current-user services. |
| `Hotel.API` | REST API (controllers), JSON converters, CORS, exception handling, and application bootstrap/DI wiring. |
| `Hotel.Shared` | Shared exception types and common primitives. |
| `Tests/Hotel.Domain.Tests` | Unit tests for domain rules and entities. |
| `Tests/Hotel.Application.Tests` | Unit tests for application handlers and behaviors. |
| `Tests/Hotel.IntegrationTests` | Integration tests covering commands, queries. |
| `Tests/Hotel.ArchitecturalTests` | Tests ensuring architectural rules are followed. |
