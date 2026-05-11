import { client } from "~/lib/api";

export async function login(credentials: { email: string; password: string }) {
  const { data, error } = await client.POST("/api/Auth/login", {
    body: credentials,
  });
  if (error) throw new Response(JSON.stringify(error), { status: 401 });
  return data;
}
