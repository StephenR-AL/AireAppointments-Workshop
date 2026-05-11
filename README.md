# AireAppointments — Workshop Challenge

This is a fun coding challenge — see how far you can get! There are no right or wrong answers; the aim is to have a go, learn something new, and build something you can be proud of.

## Overview

AireAppointments is a fictional healthcare technology company operating in West Yorkshire. This project is a **working appointment booking system** with:

- A **patient-facing form** for booking appointments
- An **admin area** behind a login screen for managing appointments
- A **React** frontend with **Tailwind CSS**
- An **ASP.NET Core** Web API backend with **Entity Framework Core** and **MySQL**
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

You'll need a MySQL instance running locally — see `appsettings.json` for the connection string.

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

**Hints:**

- Look at `frontend/app/routes/admin/dashboard.tsx` — the `appointments` array is already available
- You can calculate the counts in the component using `.filter()` and `.length`
- Style the cards using Tailwind CSS utility classes

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

### 🟡 Task 3 — Appointment Search & Filtering (Intermediate)

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

### 🟡 Task 4 — Pagination (Intermediate)

Add server-side pagination to the appointments list so it scales with large datasets.

**Requirements:**

- The API should accept `page` and `pageSize` query parameters
- Return a response that includes the **items**, **total count**, **current page**, and **total pages**
- The frontend should display **page navigation controls** (previous/next or page numbers)

**Hints:**

- Backend: use `.Skip()` and `.Take()` in EF Core
- Return a wrapper object like `{ items: [...], totalCount: 100, page: 1, pageSize: 10 }`
- Frontend: manage the current page in component state and refetch when it changes
- Consider combining this with Task 3 (search + filter + pagination)

---

### 🟡 Task 5 — Reject Appointment with Reason (Intermediate)

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

### 🟡 Task 6 — Export Appointments to CSV (Intermediate)

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
- Consider whether this endpoint should respect the current filters (if you implemented Task 3)

---

### 🔴 Task 7 — Audit Trail (Advanced)

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

### 🔴 Task 8 — Patient Email Notifications (Advanced)

Send email notifications to patients when their appointment is created and when its status changes.

**Requirements:**

- Send a **confirmation email** when a patient submits a new appointment
- Send a **status update email** when an admin approves (or rejects, if you did Task 5) an appointment
- Emails should include the patient's name, appointment date/time, and current status
- For development, emails can be **logged to the console** rather than actually sent — this is fine!

**Hints:**

- Create an `IEmailService` interface and an implementation (e.g. `ConsoleEmailService` that writes to the console/logs, or `SmtpEmailService` using MailKit for real emails)
- Register the service in `Program.cs` using dependency injection
- Call the email service from `AppointmentService` after creating or changing appointment status
- If you want to try real emails, [MailKit](https://github.com/jstedfast/MailKit) is a popular .NET library — you can use a free service like [Mailtrap](https://mailtrap.io) for testing
- Consider making the email sending async so it doesn't slow down the API response

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
