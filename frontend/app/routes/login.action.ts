import { redirect } from "react-router";
import { login } from "./login.data.client";
import type { Route } from "./+types/login";

export async function clientAction({ request }: Route.ClientActionArgs) {
  const formData = await request.formData();
  const email = formData.get("email") as string;
  const password = formData.get("password") as string;

  try {
    await login({ email, password });
    return redirect("/admin");
  } catch {
    return { error: "Invalid email or password" };
  }
}
