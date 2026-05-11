import { describe, it, expect, vi, beforeEach } from "vitest";

const { mockGET, mockPUT } = vi.hoisted(() => ({
  mockGET: vi.fn(),
  mockPUT: vi.fn(),
}));

vi.mock("~/lib/api", () => ({
  client: { GET: mockGET, PUT: mockPUT },
}));

import {
  getMe,
  getAppointment,
  updateAppointment,
} from "./edit.$id.data.client";

describe("edit.$id.data.client", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("getMe calls GET /api/Auth/me", async () => {
    mockGET.mockResolvedValueOnce({
      data: { email: "admin@test.com" },
      error: undefined,
    });

    const result = await getMe();

    expect(mockGET).toHaveBeenCalledWith("/api/Auth/me");
    expect(result).toEqual({ email: "admin@test.com" });
  });

  it("getAppointment calls GET with correct path params", async () => {
    mockGET.mockResolvedValueOnce({
      data: { id: 1, name: "Test" },
      error: undefined,
    });

    const result = await getAppointment("1");

    expect(mockGET).toHaveBeenCalledWith("/api/Appointments/{id}", {
      params: { path: { id: 1 } },
    });
    expect(result).toEqual({ id: 1, name: "Test" });
  });

  it("updateAppointment calls PUT with correct path params and body", async () => {
    const appointment = { id: 5, name: "Updated" };
    mockPUT.mockResolvedValueOnce({ data: appointment, error: undefined });

    const data = {
      name: "Updated",
      appointmentDateTime: "2026-06-01T10:00",
      description: "Updated desc",
      contactNumber: "456",
      emailAddress: "updated@test.com",
    };

    const result = await updateAppointment("5", data);

    expect(mockPUT).toHaveBeenCalledWith("/api/Appointments/{id}", {
      params: { path: { id: 5 } },
      body: data,
    });
    expect(result).toEqual(appointment);
  });
});
