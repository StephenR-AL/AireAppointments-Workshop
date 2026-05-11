import { useLoaderData, useFetcher, Link } from "react-router";

export { clientLoader } from "./dashboard.loader";
export { clientAction } from "./dashboard.action";

export default function Dashboard() {
  const { user, appointments } =
    useLoaderData<typeof import("./dashboard.loader").clientLoader>();
  const fetcher = useFetcher();

  return (
    <div className="min-h-screen bg-surface">
      <nav className="bg-white shadow-sm border-b border-border">
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <div className="flex h-16 justify-between items-center">
            <div className="flex items-center gap-8">
              <h1 className="text-xl font-bold text-gray-900">
                AireAppointments Admin
              </h1>
              <Link
                to="/admin"
                className="text-sm text-gray-600 hover:text-gray-900"
              >
                Appointments
              </Link>
            </div>
            <div className="flex items-center gap-4">
              <span className="text-sm text-gray-600">{user.email}</span>
              <fetcher.Form method="post">
                <input type="hidden" name="intent" value="logout" />
                <button
                  type="submit"
                  className="text-sm text-red-600 hover:text-red-800"
                >
                  Logout
                </button>
              </fetcher.Form>
            </div>
          </div>
        </div>
      </nav>

      <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <h2 className="text-2xl font-bold text-gray-900 mb-6">Appointments</h2>

        {appointments.length === 0 ? (
          <div className="rounded-lg bg-white p-8 text-center shadow-sm">
            <p className="text-gray-500">No appointments yet.</p>
          </div>
        ) : (
          <div className="overflow-hidden rounded-lg bg-white shadow-sm">
            <table className="min-w-full divide-y divide-border">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">
                    Name
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">
                    Date & Time
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">
                    Description
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">
                    Contact
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">
                    Email
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">
                    Status
                  </th>
                  <th className="px-6 py-3 text-right text-xs font-medium uppercase tracking-wider text-gray-500">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-border">
                {appointments.map((apt) => (
                  <tr
                    key={apt.id}
                    className={apt.status === 1 ? "bg-green-50" : ""}
                  >
                    <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-900">
                      {apt.name}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-600">
                      {new Date(apt.appointmentDateTime).toLocaleString()}
                    </td>
                    <td className="px-6 py-4 text-sm text-gray-600 max-w-xs truncate">
                      {apt.description}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-600">
                      {apt.contactNumber}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm text-gray-600">
                      {apt.emailAddress}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-sm">
                      <span
                        className={`inline-flex rounded-full px-2 py-1 text-xs font-medium ${
                          apt.status === 1
                            ? "bg-green-100 text-green-800"
                            : "bg-yellow-100 text-yellow-800"
                        }`}
                      >
                        {apt.status === 1 ? "Approved" : "Pending"}
                      </span>
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-right text-sm">
                      <div className="flex justify-end gap-2">
                        {apt.status === 0 && (
                          <fetcher.Form method="post">
                            <input
                              type="hidden"
                              name="intent"
                              value="approve"
                            />
                            <input type="hidden" name="id" value={apt.id} />
                            <button
                              type="submit"
                              className="rounded bg-success px-2 py-1 text-xs text-white hover:bg-success-hover"
                              title="Approve"
                            >
                              ✓
                            </button>
                          </fetcher.Form>
                        )}
                        <Link
                          to={`/admin/edit/${apt.id}`}
                          className="rounded bg-primary px-2 py-1 text-xs text-white hover:bg-primary-hover"
                        >
                          Edit
                        </Link>
                        <fetcher.Form method="post">
                          <input type="hidden" name="intent" value="delete" />
                          <input type="hidden" name="id" value={apt.id} />
                          <button
                            type="submit"
                            className="rounded bg-danger px-2 py-1 text-xs text-white hover:bg-danger-hover"
                          >
                            Delete
                          </button>
                        </fetcher.Form>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </main>
    </div>
  );
}
