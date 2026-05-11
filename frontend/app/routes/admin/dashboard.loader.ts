import { redirect } from "react-router";
import { getMe, getAppointments } from "./dashboard.data.client";

export async function clientLoader() {
  try {
    const user = await getMe();
    const appointments = await getAppointments();
    return { user, appointments };
  } catch {
    throw redirect("/login");
  }
}

clientLoader.hydrate = true;
