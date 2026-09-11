export interface CreateReservationRequest {
  creatorId: string;
  roomId: string;
  ratePlanId: string;
  startDate: string;
  endDate: string;
  arrivalTime: string | null;
  guestIds: string[];
}