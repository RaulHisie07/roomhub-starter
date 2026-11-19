# RoomHub (midterm starter)

Read `ASSIGNMENT.md` before anything else.

## Run

```bash
dotnet run --project src/RoomHub.Api
```

The application starts on `http://localhost:5090`. The test requests are in `roomhub.http`
(you can run them from Rider, or from VS Code with the REST Client extension).

## How to start

```bash
git clone <link-given-by-the-instructor> roomhub
cd roomhub
# create your own public repo and change the remote
git remote remove origin
git remote add origin <link-of-your-repo>
```

You can also create a new public repo and copy the contents of this folder into it.
What matters is that the repo you submit is **public** and contains your own commits.

## Refactored application

- `Features/`: five slices, each with its endpoint, handler, command/query and response.
- `Domain/`: room and booking rules, Result/Error, repository and Unit of Work contracts.
- `Infrastructure/`: EF Core repositories, SQLite mappings and Unit of Work.
- `Common/`: handler contracts and Result-to-ProblemDetails mapping.

Build and run from the repository root:

```bash
dotnet build src/RoomHub.Api
dotnet run --project src/RoomHub.Api
```

SQLite creates `roomhub.db` in the API content root at startup. Data survives
restarts. Database files are excluded from Git. To use another database, set the
`ConnectionStrings__RoomHub` environment variable to a SQLite connection string.
The initial schema is created with `EnsureCreatedAsync`; schema migrations are
not included.

Booking creation and cancellation use an immediate SQLite transaction. This
reserves the writer before checking availability and the participant's daily
count, so simultaneous requests sharing the database cannot both approve an
invalid combination. Repositories stage changes; the handler saves through
Unit of Work and commits the transaction. Failed operations roll back.

Rules R9-R11 are implemented: two active bookings per participant per UTC start
date across all rooms; a two-hour limit for capacity above ten (otherwise four);
cancellation only at least sixty minutes before the start. Exact limits are allowed.
Participant names are trimmed and compared case-sensitively.

Run fixed-clock domain and handler checks against EF Core with in-memory SQLite:

```bash
dotnet run --project tests/RoomHub.Checks
```

These checks fail with a nonzero exit code when an assertion fails. They cover
time boundaries, UTC day handling, daily limits, overlap, cancellation and
missing-room precedence. They do not use or change the application database.
