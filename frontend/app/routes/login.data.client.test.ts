import { describe, it, expect, vi, beforeEach } from "vitest";

const { mockPOST } = vi.hoisted(() => ({
  mockPOST: vi.fn(),
}));

vi.mock("~/lib/api", () => ({
  client: { POST: mockPOST },
}));

import { login } from "./login.data.client";

describe("login.data.client", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("login calls client.POST with correct credentials", async () => {
    mockPOST.mockResolvedValueOnce({
      data: { message: "Login successful" },
      error: undefined,
    });

    const result = await login({
      email: "admin@test.com",
      password: "password123",
    });

    expect(mockPOST).toHaveBeenCalledWith("/api/Auth/login", {
      body: { email: "admin@test.com", password: "password123" },
    });
    expect(result).toEqual({ message: "Login successful" });
  });

  it("login throws on invalid credentials", async () => {
    mockPOST.mockResolvedValueOnce({
      data: undefined,
      error: { message: "Invalid credentials" },
    });

    await expect(
      login({ email: "wrong@test.com", password: "wrong" }),
    ).rejects.toBeInstanceOf(Response);
  });
});
