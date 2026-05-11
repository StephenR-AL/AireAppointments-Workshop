import { client } from "~/lib/api";
import type { components } from "~/lib/api-types";

type UpdateAppointmentDto = components["schemas"]["UpdateAppointmentDto"];

export async function getMe() {
  const { data, error } = await client.GET("/api/Auth/me");
  if (error) throw new Response(JSON.stringify(error), { status: 401 });
  return data;
}

export async function getAppointment(id: string) {
  const { data, error } = await client.GET("/api/Appointments/{id}", {
    params: { path: { id: Number(id) } },
  });
  if (error) throw new Response(JSON.stringify(error), { status: 404 });
  return data;
}

export async function updateAppointment(
  id: string,
  data: UpdateAppointmentDto,
) {
  const { data: appointment, error } = await client.PUT(
    "/api/Appointments/{id}",
    {
      params: { path: { id: Number(id) } },
      body: data,
    },
  );
  if (error) throw new Response(JSON.stringify(error), { status: 400 });
  return appointment;
}
