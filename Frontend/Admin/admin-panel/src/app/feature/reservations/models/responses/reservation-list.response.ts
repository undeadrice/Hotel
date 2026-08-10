export interface ReservationListResponse {
  id: string;
  roomName: string;
  ratePlanName: string;
  creatorName: string;
  startDate: string;
  endDate: string;
  arrivalTime: string | null;
  createdAt: string;
  guestCount: number;
}
