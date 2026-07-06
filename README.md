# AireAppointments — Workshop Challenge

This is a fun coding challenge — see how far you can get! There are no right or wrong answers; the aim is to have a go, learn something new, and build something you can be proud of.

## Overview

AireAppointments is a fictional healthcare technology company operating in West Yorkshire. This project is a **working appointment booking system** with:

- A **patient-facing form** for booking appointments
- An **admin area** behind a login screen for managing appointments
- A **React** frontend with **Tailwind CSS**
- An **ASP.NET Core** Web API backend with **Entity Framework Core** and **PostgreSQL**
- **Docker Compose** for running the full stack
- **Unit tests** for both frontend and backend

Your job is to **extend this application** by tackling as many of the tasks below as you can. They're ordered by difficulty — start wherever you feel comfortable!

## Getting Started

### Running with Docker Compose

```bash
docker compose up --build
```

- Frontend: http://localhost:3000
- API + Swagger: http://localhost:5000/swagger
- Default admin login: `admin@aireappointments.co.uk` / `admin123`

### Running Locally (without Docker)

**Backend:**

```bash
cd backend
dotnet restore AireAppointments.Api.sln
dotnet run --project AireAppointments.Api
```

**Frontend:**

```bash
cd frontend
npm install
npm run dev
```

The database runs via Docker Compose. Start just the database with `docker compose up db -d`, then connect your API to it locally. You can browse the database using a tool like [DBeaver](https://dbeaver.io) or [pgAdmin](https://www.pgadmin.org) — connect to `localhost:5432` with username/password `aireappointments`.

### Running Tests

**Backend:**

```bash
cd backend
dotnet test AireAppointments.Api.sln
```

**Frontend:**

```bash
cd frontend
npm test
```

---

## Tasks

Work through as many of these as you can. Each task builds on the existing codebase — read the code, understand how it works, then extend it.

---

### 🟢 Task 1 — Dashboard Statistics Cards (Beginner)

Add summary cards at the top of the admin dashboard showing key counts.

**Requirements:**

- Display **total appointments**, **pending count**, and **approved count** as cards above the appointments table
- Use the existing appointment data already loaded by the dashboard
- Style the cards using Tailwind CSS utility classes

**Hints:**

- Look at `frontend/app/routes/admin/dashboard.tsx` — the `appointments` array is already available
- You can calculate the counts in the component using `.filter()` and `.length`

---

### 🟢 Task 2 — Confirmation Dialogs (Beginner)

Add "Are you sure?" confirmation prompts before destructive actions.

**Requirements:**

- Show a confirmation dialog before **deleting** an appointment
- Show a confirmation dialog before **approving** an appointment
- If the user cancels, the action should not proceed

**Hints:**

- Look at `frontend/app/routes/admin/dashboard.tsx` for the approve and delete buttons
- You can use the browser's built-in `window.confirm()` or build a simple modal component
- Consider preventing form submission if the user declines

---

### 🟢 Task 3 — Dark/Light Theme Toggle (Beginner)

Add a simple toggle that lets users switch between light and dark themes, with their choice remembered.

**Requirements:**

- Add a toggle button (e.g. in the admin dashboard header) that switches between Light and Dark themes
- Use Tailwind's **class-based** dark mode by applying a `dark` class on the `<html>` element
- Persist the user's choice in `localStorage` so it survives page reloads
- Apply dark styling to at least the root layout, the home page, and the admin dashboard

**Hints:**

- Tailwind v4 defaults to the `media` (OS-based) dark mode strategy. To make it toggleable by the user, add this line just below `@import "tailwindcss";` in `frontend/app/app.css`:
  ```css
  @custom-variant dark (&:where(.dark, .dark *));
  ```
  This is the v4 equivalent of `darkMode: 'class'` from Tailwind v3. There's no `tailwind.config.js` in this project — config lives in CSS.
- The `<html>` tag is rendered by the `Layout` export in `frontend/app/root.tsx` — toggle a `dark` class on it (e.g. via `document.documentElement.classList.toggle("dark", ...)`).
- Read/write the choice with `localStorage.getItem("theme")` / `localStorage.setItem("theme", "light" | "dark")`.
- Create a shared component, e.g. `frontend/app/components/ThemeToggle.tsx`. The `app/components/` directory doesn't exist yet — you'll need to create it.
- Once `dark` is on `<html>`, `dark:` variants like `dark:bg-gray-900` work. You'll need to add `dark:` variants to the existing hardcoded colours such as `bg-white`, `text-gray-900`, and `bg-gray-50` in `frontend/app/routes/admin/dashboard.tsx` and `frontend/app/routes/home.tsx`.
- The `@theme` block in `app.css` already defines tokens like `--color-surface` and `--color-border` — you can optionally override these under a `.dark` selector so any `bg-surface` / `border-border` utilities re-theme automatically.

**Note:** This is the simpler, two-way version. To also follow the operating system theme, see the intermediate version in Task 9.

---

### 🟢 Task 4 — Loading & Empty States (Beginner)

Show feedback while data is loading and when there's nothing to display.

**Requirements:**

- While appointments are loading on the admin dashboard, show a **loading indicator** (spinner or skeleton rows) instead of an empty table
- When there are **no appointments** (or the filtered list is empty), show a friendly **empty state** message
- Ensure the patient booking form shows a loading state on submit (check this — it may already be done)

**Hints:**

- Look at `frontend/app/routes/admin/dashboard.tsx` — `useLoaderData` returns the appointments. Use the `useNavigation` hook from `react-router` to detect when a loader/action is in flight (`navigation.state === "loading"`).
- For skeleton rows, a few `<div className="h-4 bg-gray-200 animate-pulse rounded" />` elements work well with Tailwind.
- Check the list length before rendering the table: `{appointments.length === 0 ? <EmptyState /> : <Table />}`. A centred message like "No appointments yet" is plenty.
- The home page in `frontend/app/routes/home.tsx` already uses `useNavigation` with `isSubmitting` to show a "Booking..." state on the submit button — use it as a reference for the pattern, and only extend it if needed.

---

### 🟡 Task 5 — Appointment Search & Filtering (Intermediate)

Add a search bar and status filter to the admin dashboard so admins can find appointments quickly.

**Requirements:**

- Add a **text search** input that filters appointments by patient name
- Add a **status filter** dropdown (All / Pending / Approved)
- Filtering should happen on the **backend** via query parameters on `GET /api/appointments`

**Hints:**

- Backend: modify `AppointmentService.GetAllAsync()` to accept optional `search` and `status` parameters, use `.Where()` LINQ queries
- Backend: update the controller to read query string parameters
- Frontend: add filter controls above the table and pass query parameters when fetching data
- Look at `frontend/app/routes/admin/dashboard.data.client.ts` for how API calls are made

---

### 🟡 Task 6 — Pagination (Intermediate)

Add server-side pagination to the appointments list so it scales with large datasets.

**Requirements:**

- The API should accept `page` and `pageSize` query parameters
- Return a response that includes the **items**, **total count**, **current page**, and **total pages**
- The frontend should display **page navigation controls** (previous/next or page numbers)

**Hints:**

- Backend: use `.Skip()` and `.Take()` in EF Core
- Return a wrapper object like `{ items: [...], totalCount: 100, page: 1, pageSize: 10 }`
- Frontend: manage the current page in component state and refetch when it changes
- Consider combining this with Task 5 (search + filter + pagination)

---

### 🟡 Task 7 — Reject Appointment with Reason (Intermediate)

Add the ability for admins to reject an appointment with a reason, expanding the status workflow.

**Requirements:**

- Add a **Rejected** value to the `AppointmentStatus` enum
- Add a `RejectionReason` field to the `Appointment` model
- Create a `PATCH /api/appointments/{id}/reject` endpoint that accepts a rejection reason
- Show a **Reject button** in the admin dashboard that prompts for a reason
- Display rejected appointments with a **distinct colour** in the table (e.g. red background)
- Show the rejection reason somewhere visible (tooltip, extra column, or expandable row)

**Hints:**

- Look at how the existing `ApproveAsync` method works in `AppointmentService.cs` — follow the same pattern
- You'll need a new DTO for the rejection request (e.g. `RejectAppointmentDto` with a `Reason` field)
- Update `AppointmentStatus.cs` to add `Rejected = 2`
- On the frontend, use conditional Tailwind classes based on `appointment.status`

---

### 🟡 Task 8 — Export Appointments to CSV (Intermediate)

Add the ability for admins to download all appointments as a CSV file.

**Requirements:**

- Add a **"Download CSV"** button to the admin dashboard
- Create a `GET /api/appointments/export` endpoint that returns a CSV file
- The CSV should include all appointment fields (Name, Date, Description, Contact, Email, Status)
- The file should download with an appropriate filename (e.g. `appointments-2026-05-11.csv`)

**Hints:**

- Backend: return a `FileContentResult` with content type `text/csv`
- Build the CSV string manually or use a library like `CsvHelper`
- Set the `Content-Disposition` header for the filename
- Frontend: use `window.location.href` or a direct `<a>` link to trigger the download
- Consider whether this endpoint should respect the current filters (if you implemented Task 5)

---

### 🟡 Task 9 — Theme Toggle with System Detection (Intermediate)

Extend the simple toggle from Task 3 with a "System" option that follows the operating system theme, with no flash on load.

**Requirements:**

- Offer **three** options: Light, Dark, and System (a segmented control or 3-way dropdown)
- When "System" is selected, the theme follows `prefers-color-scheme: dark` from the OS
- If the user changes their OS theme while "System" is selected, the app should **update live** without a page reload
- Persist the chosen _mode_ (`light` / `dark` / `system`) in `localStorage`
- **No flash of the wrong theme** on initial page load — the correct theme must be applied before React renders

**Hints:**

- Build on Task 3. Detect the OS theme with `window.matchMedia('(prefers-color-scheme: dark)')` and subscribe to changes:
  ```ts
  const mql = window.matchMedia("(prefers-color-scheme: dark)");
  mql.addEventListener("change", handler);
  ```
- Avoiding a flash is the tricky part. Because this app is a **SPA** (`ssr: false` in `frontend/react-router.config.ts`), React hasn't rendered on first paint, so a `useEffect`-based toggle runs too late. Add an **inline `<script>` in the `<head>`** of `frontend/app/root.tsx` (inside the `Layout` export, before `<Scripts />`) that synchronously sets the class before the page paints:
  ```tsx
  <script dangerouslySetInnerHTML={{ __html: `
    (function () {
      var mode = localStorage.getItem("theme") || "system";
      var dark = mode === "dark" ||
        (mode === "system" && window.matchMedia("(prefers-color-scheme: dark)").matches);
      document.documentElement.classList.toggle("dark", dark);
    })();
  ` }} />
  ```
- Keep the logic in a small hook, e.g. `frontend/app/lib/useTheme.ts`, returning `{ mode, setMode, resolvedTheme }` and subscribing to `matchMedia` changes. A `useSyncExternalStore`-based hook is a clean way to stay in sync with the inline script and OS changes; plain `useState` + an event listener also works.
- Update the `ThemeToggle` component from Task 3 to render three options instead of a single switch.
- Write a test for the hook/component using the existing Vitest setup — see `frontend/app/routes/admin/dashboard.data.client.test.ts` for the pattern and `frontend/test/setup.ts` for setup. Note: jsdom doesn't ship `matchMedia`, so you'll need to stub it in the test.

---

### 🟡 Task 10 — Database Migrations (Intermediate)

Replace `EnsureCreatedAsync` with real EF Core migrations so the schema can evolve safely.

**Requirements:**

- Replace the `db.Database.EnsureCreatedAsync()` call in `Program.cs` with `db.Database.MigrateAsync()`
- Generate an **initial migration** that captures the current schema (Appointments + Admins tables)
- Keep `DbSeeder.Seed` running after migration (verify it's still idempotent)
- Verify the app still starts cleanly against a fresh PostgreSQL container

**Hints:**

- The `Microsoft.EntityFrameworkCore.Design` 8.0.0 package is already referenced in `backend/AireAppointments.Api/AireAppointments.Api.csproj` — you only need the `dotnet-ef` tool.
- Install it if you don't have it: `dotnet tool install --global dotnet-ef`.
- From `backend/`, scaffold the initial migration:
  ```bash
  dotnet ef migrations add InitialCreate --project AireAppointments.Api --output-dir Migrations
  ```
  This creates a `Migrations/` folder inside `AireAppointments.Api` (it doesn't exist yet — there are currently no migrations in the repo).
- In `backend/AireAppointments.Api/Program.cs`, swap the `EnsureCreatedAsync` call (inside the retry loop) for `await db.Database.MigrateAsync();`. Keep the retry loop — the Postgres container may still take a moment to be ready in Docker Compose.
- `DbSeeder.Seed` already checks `if (!context.Admins.Any())`, so it's safe to run after a migration.
- The tests in `backend/AireAppointments.Api.Tests/` use the EF Core **InMemory** provider (see `TestHelper.cs`), so they're unaffected — no test changes needed.
- Bonus: add a migration step to the backend `Dockerfile` so the container applies migrations on startup.

---

### 🟡 Task 11 — Prevent Double-Booking (Intermediate)

Stop patients from booking an appointment at a date/time that's already taken.

**Requirements:**

- Before creating an appointment, check the database for an existing appointment at the same time (or within a configurable window, e.g. ±30 minutes)
- If a conflict is found, reject the request with **HTTP 409 Conflict** instead of creating a duplicate
- Show a friendly error message on the patient booking form when this happens
- Add a unit test for the conflict check

**Hints:**

- The current `CreateAsync` in `backend/AireAppointments.Api/Services/AppointmentService.cs` simply builds an `Appointment` and calls `SaveChangesAsync` — there's no conflict check.
- Add a LINQ check before inserting, e.g. `await db.Appointments.AnyAsync(a => Math.Abs((a.AppointmentDateTime - dto.AppointmentDateTime).TotalMinutes) < 30)`. Adjust the window to taste.
- Decide how to signal the conflict: throw a custom `BookingConflictException`, or change `CreateAsync` to return a result type. Update the `Create` action in `backend/AireAppointments.Api/Controllers/AppointmentsController.cs` to return `Conflict(new { message = "..." })` on a conflict.
- Add a test in `backend/AireAppointments.Api.Tests/AppointmentServiceTests.cs` mirroring the existing pattern — `TestHelper.CreateInMemoryContext()`, seed an appointment at a time, attempt to create another at the same time, and assert it fails (FluentAssertions).
- On the frontend, the booking form in `frontend/app/routes/home.tsx` submits via a `<Form method="post">` and reads `actionData.errors` for inline errors — extend `frontend/app/routes/home.action.ts` to surface the 409 message into `actionData.errors.message`.

---

### 🔴 Task 12 — Audit Trail (Advanced)

Track all admin actions so there's a history of who did what and when.

**Requirements:**

- Create a new `AuditLog` model with fields: `Id`, `AdminEmail`, `Action` (e.g. "Approved", "Deleted", "Edited"), `AppointmentId`, `Details`, `Timestamp`
- Create a corresponding database table (add it to `AppDbContext`)
- Log an entry whenever an admin **approves**, **edits**, or **deletes** an appointment
- Create a `GET /api/audit` endpoint to retrieve audit logs (admin-only)
- Build a new **Audit Log page** in the admin area accessible from the navigation bar
- Display the logs in a table sorted by most recent first

**Hints:**

- Create an `AuditService` with a `LogAsync(string adminEmail, string action, int appointmentId, string? details)` method
- Inject the audit service into `AppointmentsController` and call it in the relevant action methods
- Get the admin email from `HttpContext.Items["AdminEmail"]`
- Frontend: add a new route (e.g. `/admin/audit`) and register it in `frontend/app/routes.ts`
- Follow the same patterns as the existing dashboard page for loaders and data fetching

---

### 🔴 Task 13 — Patient Email Notifications (Advanced)

Send email notifications to patients when their appointment is created and when its status changes.

**Requirements:**

- Send a **confirmation email** when a patient submits a new appointment
- Send a **status update email** when an admin approves (or rejects, if you did Task 7) an appointment
- Emails should include the patient's name, appointment date/time, and current status
- For development, emails can be **logged to the console** rather than actually sent — this is fine!

**Hints:**

- Create an `IEmailService` interface and an implementation (e.g. `ConsoleEmailService` that writes to the console/logs, or `SmtpEmailService` using MailKit for real emails)
- Register the service in `Program.cs` using dependency injection
- Call the email service from `AppointmentService` after creating or changing appointment status
- If you want to try real emails, [MailKit](https://github.com/jstedfast/MailKit) is a popular .NET library — you can use a free service like [Mailtrap](https://mailtrap.io) for testing
- Consider making the email sending async so it doesn't slow down the API response

---

### 🔴 Task 14 — Security Hardening (Advanced)

Tighten the app's security posture — the current setup is intentionally permissive for local dev.

**Requirements:**

- **CORS:** Replace the permissive `SetIsOriginAllowed(_ => true)` policy in `Program.cs` with an explicit allowed origin read from `appsettings.json` (e.g. a `"FrontendOrigin"` setting), still allowing credentials
- **Auth cookie:** In `backend/AireAppointments.Api/Controllers/AuthController.cs`, set `Secure = true` and `SameSite = SameSiteMode.Strict` (or `Lax`) on the `aireappointments_auth` cookie. Make `Secure` config-driven so local HTTP dev still works.
- **Rate limiting:** Add ASP.NET Core's built-in rate limiting (`AddRateLimiter`) to throttle the public `POST /api/appointments` and `POST /api/auth/login` endpoints (e.g. fixed window per IP)
- **Validation:** Add `[MaxLength]` attributes to the DTOs in `backend/AireAppointments.Api/DTOs/` to match the `HasMaxLength` constraints in `AppDbContext.OnModelCreating`, so over-long input is rejected with a 400 instead of a 500 from the database

**Hints:**

- The CORS policy is named `"AllowFrontend"` in `Program.cs`. Change `SetIsOriginAllowed(_ => true)` to `WithOrigins(builder.Configuration["FrontendOrigin"]!)` and add `"FrontendOrigin": "http://localhost:3000"` to `appsettings.json`. (Add the production origin in a separate environment settings file.)
- `CookieOptions` in `AuthController.Login` currently has `Secure = false, SameSite = SameSiteMode.Lax`. Read `Secure` from config so HTTP local dev still works, defaulting to `true` in production.
- Rate limiting docs: <https://learn.microsoft.com/aspnet/core/performance/rate-limit> — call `builder.Services.AddRateLimiter(...)` and `app.UseRateLimiter()` in the pipeline, then apply `[EnableRateLimiting("policy")]` to the login and create actions. Put `UseRateLimiter` early in the pipeline, and be careful not to throttle admin read endpoints.
- The DTO/entity mismatch: `CreateAppointmentDto.cs` and `UpdateAppointmentDto.cs` have `[Required]` but no `[MaxLength]`, while `AppDbContext` sets `HasMaxLength(200)` on Name/EmailAddress and `HasMaxLength(1000)` on Description. Add matching `[MaxLength(...)]` attributes and a validation test in `AppointmentDtoValidationTests.cs` for the new limits.

---

### 🔴 Task 15 — Response DTOs & Mapping (Advanced)

Stop returning raw entity objects from the API and introduce a proper mapping layer.

**Requirements:**

- Create response DTOs (e.g. `AppointmentResponseDto`) that expose only the fields the frontend needs — decide deliberately whether `CreatedAt` should be exposed
- Create a static mapping class (e.g. `AppointmentMapper`) that converts `Appointment` entities to response DTOs
- Update **every** endpoint in `AppointmentsController` to return the response DTO instead of the raw entity
- Regenerate the frontend API types with `npm run generate:types` and fix any resulting type errors
- Update the controller tests to expect the DTO shape

**Hints:**

- `AppointmentsController` currently does `return Ok(appointment);` with the raw entity — this couples the API to the database schema and leaks internal fields.
- Put the new DTO in `backend/AireAppointments.Api/DTOs/` alongside `CreateAppointmentDto.cs`. A static `AppointmentMapper.ToDto(Appointment entity)` is fine for this size of app — no need for AutoMapper, though you can use it if you'd like to demonstrate the pattern.
- The frontend consumes the API via `openapi-fetch` in `frontend/app/lib/api.ts`, with TypeScript types generated from Swagger in `frontend/app/lib/api-types.d.ts`. After changing the response shape, run `npm run generate:types` (see `frontend/package.json`) and fix any type errors. The API must be running on port 5000 for this script to fetch the swagger JSON.
- Update the controller tests in `AppointmentsControllerTests.cs` — they currently mock `IAppointmentService` and assert on `OkObjectResult.Value`; update them to expect the DTO shape, and have the service layer return entities which the controller maps (or vice versa — pick one and be consistent).
- Consider whether `UpdateAppointmentDto` should also be able to set `Status` (currently it can't, since the DTO has no `Status` field and `UpdateAsync` doesn't touch it).

---

### 🔴 Task 16 — Admin User Management (Advanced)

Let the seeded admin manage other admins so the system isn't single-user.

**Requirements:**

- Create a new `AdminsController` with admin-only endpoints: list admins, create a new admin (email + password), and delete an admin
- Reuse the existing `Admin` model and BCrypt password hashing from `AuthService`
- Add an **Admin Management** page in the frontend admin area, linked from the dashboard navigation
- Don't allow an admin to delete themselves
- Add backend tests for the new service methods

**Hints:**

- The `Admin` model already exists in `backend/AireAppointments.Api/Models/Admin.cs` with `Id`, `Email`, and `PasswordHash`. `AuthService` shows how to BCrypt-hash a password — extract a small `IAdminService` (or add methods to `AuthService`) for `GetAllAsync`, `CreateAsync(email, password)`, `DeleteAsync(id)`.
- Register the new service in `Program.cs` following the existing `AddScoped<...>` pattern.
- The `[Authorize]` attribute on `AppointmentsController` shows how admin-only endpoints are protected — apply the same to the new controller.
- For the frontend, add a route in `frontend/app/routes.ts` (e.g. `route("admin/admins", "routes/admin/admins.tsx")`) and follow the split-file pattern used by `dashboard.tsx` / `dashboard.loader.ts` / `dashboard.data.client.ts` (the same pattern as Task 12's audit page).
- Get the current admin's id from `HttpContext.Items["AdminId"]` (set by `AuthMiddleware`) to prevent self-deletion — return `BadRequest` if the target id equals the current admin's id.
- `DbSeeder` only seeds when there are no admins, so newly-created admins won't be wiped on restart.

---

### 🔴 Task 17 — Enable Server-Side Rendering (Advanced)

Switch the frontend from a pure client-side SPA to server-side rendering with React Router v7, so the first paint arrives as real HTML instead of a blank page waiting for hydration.

**Requirements:**

- Enable SSR by flipping `ssr: false` to `ssr: true` in `frontend/react-router.config.ts`
- Add a server entry (`frontend/app/entry.server.tsx`) that renders the app to a stream/string
- Convert at least the dashboard and home routes to use server-side `loader` functions so their data is fetched on the server before render
- **Forward the auth cookie** so authenticated pages render correctly on the server (not just the client)
- Make the API base URL configurable via an environment variable so it works both locally and in Docker
- Update the production `start` script and `frontend/Dockerfile` to serve the SSR bundle instead of static files
- Verify the app still works end-to-end: initial load shows real HTML, hydration completes, login/logout still functions, and the dashboard renders server-side when logged in

**Hints:**

- **The config flip:** `frontend/react-router.config.ts` is currently `{ ssr: false }` — change it to `ssr: true`. The `@react-router/node` and `@react-router/serve` packages are already in `package.json`, so you have the Node SSR adapter and server ready — no new dependencies needed.
- **Server entry:** There's no `frontend/app/entry.server.tsx` yet (only `entry.client.tsx`, which is already SSR-compatible and needs no change). Create one that exports a `default` handler using `handleRequest` from `@react-router/node` and `renderToPipeableStream` (or `renderToString`) from `react-dom/server`. See the React Router v7 SSR guide: <https://reactrouter.com/start/framework/route-module> — the framework will scaffold a default `entry.server.tsx` if you run the dev server after enabling `ssr: true`, which you can then customise.
- **Loaders — the core refactor:** Every route currently exports `clientLoader` (from `*.loader.ts` files) because `ssr: false` required it. With SSR on, add server-side `loader` exports alongside them so data is fetched on the server. You can keep `clientLoader` for subsequent client-side navigations. Look at `frontend/app/routes/admin/dashboard.loader.ts` — it calls `getMe()` and `getAppointments()` from `dashboard.data.client.ts`. Create a server `loader` that does the same but forwards the request cookie.
- **Cookie forwarding — the tricky part:** `frontend/app/lib/api.ts` creates an openapi-fetch `client` with `credentials: "include"` and a hardcoded `http://localhost:5000` base. On the server there's no browser cookie jar, so `getMe()` will 401 on every server render and redirect to `/login`. The server `loader` receives the incoming `request`, so read `request.headers.get("Cookie")` and pass it through to your fetch calls (e.g. via a `headers: { Cookie: ... }` option, or a second server-oriented client instance). Without this, the dashboard can never render server-side for logged-in users.
- **Configurable API URL:** `http://localhost:5000` won't resolve from inside the frontend Docker container (the backend is a separate service). Read the base URL from an env var, e.g. `const API_BASE = process.env.API_BASE_URL ?? "http://localhost:5000";`, and set `API_BASE_URL=http://backend:5000` (or whatever the compose service is called) in `frontend/Dockerfile` / `docker-compose.yml`. On the server you'll want an _internal_ URL; in the browser you may still want the _external_ one — consider two values.
- **Serving the SSR build:** `npm start` currently runs `serve ./build/client -l 3000 -s` (static SPA hosting). With SSR on, `react-router build` emits both `build/client` and `build/server`. Change `start` in `package.json` to `react-router-serve ./build/server/index.js` (the `@react-router/serve` package provides this binary). Update `frontend/Dockerfile` to copy `build/server` (and `build/client`) into the runtime image and run the SSR server.
- **Hydration & fallbacks:** `frontend/app/root.tsx` already exports `HydrateFallback` and `ErrorBoundary` — these continue to work under SSR. The `clientLoader.hydrate = true` flags in the existing loaders become relevant: decide whether you still need client-side hydration loaders once server loaders exist.
- **Theme/FOUC (if you did Task 9):** the inline `<head>` script still applies the `dark` class before paint, so no flash. For a truly server-rendered theme you could read a theme cookie in `entry.server.tsx` and set the class on the streamed `<html>`, but the inline-script approach from Task 9 keeps working as-is.
- **Tests:** the frontend Vitest tests in `frontend/app/routes/**/*.data.client.test.ts` call the data functions directly and don't depend on SSR, so they should keep passing. The backend is unchanged.

---

## Source Control

- We recommend hosting your project on a **public GitHub repository** so you can show off your work
- Try to commit after completing each task — one commit per task is a great habit
- A clear commit history is a great way to show your progress

## Additional Notes

- Try to make your solution:
  - Functional for users
  - Readable and maintainable for other developers
  - Easy to extend in the future
- Feel free to use AI coding tools if you'd like — we just encourage you to be ready to talk through the decisions you made along the way
- Write tests for your changes where you can — look at the existing tests for examples
- Aim to meet the core requirements of each task rather than over-engineering
