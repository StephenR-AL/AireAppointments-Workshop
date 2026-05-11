import { redirect } from "react-router";
import { getMe, getAppointment } from "./edit.$id.data.client";

export async function clientLoader({ params }: { params: { id: string } }) {
  try {
    await getMe();
    const appointment = await getAppointment(params.id);
    return { appointment };
  } catch {
    throw redirect("/login");
  }
}

clientLoader.hydrate = true;
