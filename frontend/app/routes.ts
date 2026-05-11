import { type RouteConfig, index, route } from "@react-router/dev/routes";

export default [
  index("routes/home.tsx"),
  route("login", "routes/login.tsx"),
  route("admin", "routes/admin/dashboard.tsx"),
  route("admin/edit/:id", "routes/admin/edit.$id.tsx"),
] satisfies RouteConfig;
