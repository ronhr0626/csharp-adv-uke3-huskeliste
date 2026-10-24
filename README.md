# CSharp Adv – Week 3: Who shall pass? (JWT)

A to-do API with login and JWT.

- Users register and log in to get a token.
- Each user only sees and changes their own tasks.
- Builds on week 2 (Docker, Traefik, PostgreSQL, EF Core).

## Authentication flow

The diagram is in `Excalidraw/AuthFlow.excalidraw` (and as an image in `Excalidraw/AuthFlow.png`). It shows the four roles:

| Role | In this project |
|---|---|
| **Resource Owner** | The user (e.g. daddy) who owns their tasks |
| **Client** | Scalar or curl, which sends the requests |
| **Identity Provider** | `AuthController` (`/auth/register` and `/auth/login`), which checks the password and creates the token |
| **Resource Server** | `TaskController` (`/tasks`), which reads the token and gives access to the right data |

```
1. POST /auth/register  →  the user is saved with a BCrypt hash of the password
2. POST /auth/login     →  the password is verified and the API returns a JWT
3. The client sends     Authorization: Bearer <token>
4. TaskController reads "sub" (user id) from the token
5. Service and repository filter on that id
```

## Data ownership rules

1. Two users can both have a task called "buy milk", but each user only sees their own tasks.
2. A user can only change or delete their own tasks.
3. Without a valid token, no tasks can be reached at all.

Another user's task gives **404 Not