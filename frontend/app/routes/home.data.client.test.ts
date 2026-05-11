import { describe, it, expect, vi, beforeEach } from "vitest";

const { mockPOST } = vi.hoisted(() => ({
  mockPOST: vi.fn(),
}));

vi.mock("~/lib/api", () => ({
  client: { POST: mockPOST },
}));

import { createAppointment } from "./home.data.client";

describe("home.data.client", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("createAppointment calls client.POST with correct path and body", async () => {
    const appointment = { id: 1, name: "John" };
    mockPOST.mockResolvedValueOnce({ data: appointment, error: undefined });

    const data = {
      name: "John",
      appointmentDateTime: "2026-05-01T10:00",
      description: "Checkup",
      contactNumber: "07700900000",
      emailAddress: "john@test.com",
    };

    const result = await createAppointment(data);

    expect(mockPOST).toHaveBeenCalledWith("/api/Appointments", { body: data });
    expect(result).toEqual(appointment);
  });

  it("createAppointment throws on api error", async () => {
    mockPOST.mockResolvedValueOnce({
      data: undefined,
      error: { message: "Bad request" },
    });

    await expect(
      createAppointment({
        name: "John",
        appointmentDateTime: "2026-05-01T10:00",
        description: "Checkup",
        contactNumber: "07700900000",
        emailAddress: "john@test.com",
      }),
    ).rejects.toBeInstanceOf(Response);
  });
});
