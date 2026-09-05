import { CreateRatePlanRoomRequest } from './create-rate-plan-room.request';

export interface CreateRatePlanRequest {
  name: string;
  transactionCodeId: string;
  startDate: string;
  endDate: string;
  rooms: CreateRatePlanRoomRequest[];
}