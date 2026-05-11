import { describe, it, expect, vi, beforeEach } from "vitest";

const { mockGET, mockPOST, mockPATCH, mockDELETE } = vi.hoisted(() => ({
  mockGET: vi.fn(),
  mockPOST: vi.fn(),
  mockPATCH: vi.fn(),
  mockDELETE: vi.fn(),
}));

vi.mock("~/lib/api", () => ({
  client: {
    GET: mockGET,
    POST: mockPOST,
    PATCH: mockPATCH,
    DELETE: mockDELETE,
  },
}));

import {
  getMe,
  getAppointments,
  approveAppointment,
  deleteAppointment,
  logout,
} from "./dashboard.data.client";

describe("dashboard.data.client", () => {
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

  it("getAppointments calls GET /api/Appointments", async () => {
    mockGET.mockResolvedValueOnce({ data: [], error: undefined });

    const result = await getAppointments();

    expect(mockGET).toHaveBeenCalledWith("/api/Appointments");
    expect(result).toEqual([]);
  });

  it("approveAppointment calls PATCH with correct path params", async () => {
    mockPATCH.mockResolvedValueOnce({ error: undefined });

    await approveAppointment("1");

    expect(mockPATCH).toHaveBeenCalledWith("/api/Appointments/{id}/approve", {
      params: { path: { id: 1 } },
    });
  });

  it("deleteAppointment calls DELETE with correct path params", async () => {
    mockDELETE.mockResolvedValueOnce({ error: undefined });

    await deleteAppointment("1");

    expect(mockDELETE).toHaveBeenCalledWith("/api/Appointments/{id}", {
      params: { path: { id: 1 } },
    });
  });

  it("logout calls POST /api/Auth/logout", async () => {
    mockPOST.mockResolvedValueOnce({ error: undefined });

    await logout();

    expect(mockPOST).toHaveBeenCalledWith("/api/Auth/logout");
  });
});
