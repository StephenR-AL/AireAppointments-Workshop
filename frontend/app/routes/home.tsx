import { useActionData, useNavigation, Form, Link } from "react-router";

export { clientAction } from "./home.action";

function getMinDatetimeLocal() {
  const now = new Date();
  now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
  return now.toISOString().slice(0, 16);
}

export default function Home() {
  const actionData =
    useActionData<typeof import("./home.action").clientAction>();
  const navigation = useNavigation();
  const isSubmitting = navigation.state === "submitting";

  if (actionData?.success) {
    return (
      <div className="flex min-h-screen items-center justify-center bg-surface px-4">
        <div className="w-full max-w-md rounded-lg bg-white p-8 shadow-md text-center">
          <div className="mb-4 text-5xl">✓</div>
          <h1 className="text-2xl font-bold text-gray-900">
            Appointment Booked
          </h1>
          <p className="mt-2 text-gray-600">
            We will be in touch to confirm your appointment.
          </p>
          <a
            href="/"
            className="mt-6 inline-block rounded-md bg-primary px-4 py-2 text-white hover:bg-primary-hover"
          >
            Book Another
          </a>
        </div>
      </div>
    );
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-surface px-4 py-12">
      <div className="w-full max-w-md">
        <div className="mb-8 text-center">
          <h1 className="text-3xl font-bold text-gray-900">
            Book an Appointment
          </h1>
          <p className="mt-2 text-gray-600">AireAppointments HealthTech</p>
          <Link
            to="/login"
            className="mt-4 inline-block rounded-md border border-border px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
          >
            Admin Login
          </Link>
        </div>

        <Form
          method="post"
          className="space-y-5 rounded-lg bg-white p-8 shadow-md"
        >
          {actionData?.errors?.message && (
            <div className="rounded-md bg-red-50 p-3 text-sm text-red-700">
              {actionData.errors.message}
            </div>
          )}

          <div>
            <label
              htmlFor="name"
              className="block text-sm font-medium text-gray-700"
            >
              Name
            </label>
            <input
              id="name"
              name="name"
              type="text"
              required
              className="mt-1 block w-full rounded-md border border-border px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
            />
          </div>

          <div>
            <label
              htmlFor="appointmentDateTime"
              className="block text-sm font-medium text-gray-700"
            >
              Appointment Date & Time
            </label>
            <input
              id="appointmentDateTime"
              name="appointmentDateTime"
              type="datetime-local"
              required
              min={getMinDatetimeLocal()}
              className="mt-1 block w-full rounded-md border border-border px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
            />
            {actionData?.errors?.appointmentDateTime && (
              <p className="mt-1 text-sm text-red-600">
                {actionData.errors.appointmentDateTime}
              </p>
            )}
          </div>

          <div>
            <label
              htmlFor="description"
              className="block text-sm font-medium text-gray-700"
            >
              Description
            </label>
            <textarea
              id="description"
              name="description"
              rows={3}
              required
              className="mt-1 block w-full rounded-md border border-border px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
            />
          </div>

          <div>
            <label
              htmlFor="contactNumber"
              className="block text-sm font-medium text-gray-700"
            >
              Contact Number
            </label>
            <input
              id="contactNumber"
              name="contactNumber"
              type="tel"
              required
              pattern="\d*"
              inputMode="numeric"
              className="mt-1 block w-full rounded-md border border-border px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
            />
            {actionData?.errors?.contactNumber && (
              <p className="mt-1 text-sm text-red-600">
                {actionData.errors.contactNumber}
              </p>
            )}
          </div>

          <div>
            <label
              htmlFor="emailAddress"
              className="block text-sm font-medium text-gray-700"
            >
              Email Address
            </label>
            <input
              id="emailAddress"
              name="emailAddress"
              type="email"
              required
              className="mt-1 block w-full rounded-md border border-border px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
            />
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="w-full rounded-md bg-primary px-4 py-2.5 text-white shadow-sm hover:bg-primary-hover focus:outline-none focus:ring-2 focus:ring-primary focus:ring-offset-2 disabled:opacity-50"
          >
            {isSubmitting ? "Booking..." : "Book Appointment"}
          </button>
        </Form>
      </div>
    </div>
  );
}
