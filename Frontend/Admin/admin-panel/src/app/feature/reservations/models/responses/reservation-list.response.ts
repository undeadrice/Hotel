export interface ReservationListResponse {
  id: string;
  creatorId: string;
  roomId: string;
  ratePlanId: string;
  startDate: string;
  endDate: string;
  arrivalTime: string | null;
  createdAt: string;
  guestIds: string[];
}