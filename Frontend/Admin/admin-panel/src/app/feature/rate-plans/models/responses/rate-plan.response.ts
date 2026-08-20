import { RatePlanRoomResponse } from './rate-plan-room.response';

export interface RatePlanResponse {
  id: string;
  name: string;
  transactionCode: string;
  startDate: string;
  endDate: string;
  rooms: RatePlanRoomResponse[];
}