import { ReservationStatus } from '../../enums/reservation-status.enum';

export interface ReservationListResponse {
  id: string;
  cycleIdentifier: string;
  roomName: string;
  ratePlanName: string;
  creatorName: string;
  startDate: string;
  endDate: string;
  arrivalTime: string | null;
  createdAt: string;
  status: ReservationStatus;
  guestCount: number;
}
