import { client } from "~/lib/api";
import type { components } from "~/lib/api-types";

type CreateAppointmentDto = components["schemas"]["CreateAppointmentDto"];

export async function createAppointment(data: CreateAppointmentDto) {
  const { data: appointment, error } = await client.POST("/api/Appointments", {
    body: data,
  });

  if (error) {
    throw new Response(JSON.stringify(error), { status: 400 });
  }

  return appointment;
}
