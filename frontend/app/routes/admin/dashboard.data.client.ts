import { client } from "~/lib/api";

export async function getMe() {
  const { data, error } = await client.GET("/api/Auth/me");
  if (error) throw new Response(JSON.stringify(error), { status: 401 });
  return data;
}

export async function getAppointments() {
  const { data, error } = await client.GET("/api/Appointments");
  if (error) throw new Response(JSON.stringify(error), { status: 500 });
  return data;
}

export async function approveAppointment(id: FormDataEntryValue | null) {
  const { error } = await client.PATCH("/api/Appointments/{id}/approve", {
    params: { path: { id: Number(id) } },
  });
  if (error) throw new Response(JSON.stringify(error), { status: 400 });
}

export async function deleteAppointment(id: FormDataEntryValue | null) {
  const { error } = await client.DELETE("/api/Appointments/{id}", {
    params: { path: { id: Number(id) } },
  });
  if (error) throw new Response(JSON.stringify(error), { status: 400 });
}

export async function logout() {
  const { error } = await client.POST("/api/Auth/logout");
  if (error) throw new Response(JSON.stringify(error), { status: 500 });
}
