import { redirect } from "react-router";
import {
  approveAppointment,
  deleteAppointment,
  logout,
} from "./dashboard.data.client";
import type { Route } from "./+types/dashboard";

export async function clientAction({ request }: Route.ClientActionArgs) {
  const formData = await request.formData();
  const intent = formData.get("intent");
  const id = formData.get("id");

  if (intent === "approve") {
    await approveAppointment(id);
  } else if (intent === "delete") {
    await deleteAppointment(id);
  } else if (intent === "logout") {
    await logout();
    return redirect("/login");
  }

  return { ok: true };
}
