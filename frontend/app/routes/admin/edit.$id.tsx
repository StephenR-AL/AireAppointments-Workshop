import {
  useLoaderData,
  useActionData,
  useNavigation,
  Form,
  Link,
} from "react-router";

export { clientLoader } from "./edit.$id.loader";
export { clientAction } from "./edit.$id.action";

function getMinDatetimeLocal() {
  const now = new Date();
  now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
  return now.toISOString().slice(0, 16);
}

export default function EditAppointment() {
  const { appointment } =
    useLoaderData<typeof import("./edit.$id.loader").clientLoader>();
  const actionData =
    useActionData<typeof import("./edit.$id.action").clientAction>();
  const navigation = useNavigation();
  const isSubmitting = navigation.state === "submitting";

  const formatDateForInput = (iso: string) => {
    return iso.slice(0, 16);
  };

  return (
    <div className="min-h-screen bg-surface">
      <nav className="bg-white shadow-sm border-b border-border">
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <div className="flex h-16 items-center gap-8">
            <h1 className="text-xl font-bold text-gray-900">
              AireAppointments Admin
            </h1>
            <Link
              to="/admin"
              className="text-sm text-gray-600 hover:text-gray-900"
            >
              ← Back to Appointments
            </Link>
          </div>
        </div>
      </nav>

      <main className="mx-auto max-w-2xl px-4 py-8 sm:px-6 lg:px-8">
        <h2 className="text-2xl font-bold text-gray-900 mb-6">
          Edit Appointment
        </h2>

        <Form
          method="post"
          className="space-y-5 rounded-lg bg-white p-8 shadow-md"
        >
          {actionData?.error && (
            <div className="rounded-md bg-red-50 p-3 text-sm text-red-700">
              {actionData.error}
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
              defaultValue={appointment.name}
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
              defaultValue={formatDateForInput(appointment.appointmentDateTime)}
              className="mt-1 block w-full rounded-md border border-border px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
            />
            {actionData?.error && actionData.error.includes("future") && (
              <p className="mt-1 text-sm text-red-600">
                Appointment must be in the future
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
              defaultValue={appointment.description}
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
              defaultValue={appointment.contactNumber}
              className="mt-1 block w-full rounded-md border border-border px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
            />
            {actionData?.error && actionData.error.includes("digits") && (
              <p className="mt-1 text-sm text-red-600">
                Contact number must contain only digits
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
              defaultValue={appointment.emailAddress}
              className="mt-1 block w-full rounded-md border border-border px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
            />
          </div>

          <div className="flex gap-3">
            <button
              type="submit"
              disabled={isSubmitting}
              className="rounded-md bg-primary px-4 py-2.5 text-white shadow-sm hover:bg-primary-hover focus:outline-none focus:ring-2 focus:ring-primary focus:ring-offset-2 disabled:opacity-50"
            >
              {isSubmitting ? "Saving..." : "Save Changes"}
            </button>
            <Link
              to="/admin"
              className="rounded-md border border-border px-4 py-2.5 text-gray-700 shadow-sm hover:bg-gray-50"
            >
              Cancel
            </Link>
          </div>
        </Form>
      </main>
    </div>
  );
}
