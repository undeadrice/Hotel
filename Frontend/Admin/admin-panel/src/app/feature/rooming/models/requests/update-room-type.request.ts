export interface UpdateRoomTypeRequest {
  id: string;
  name: string;
  baseRate: number;
  description: string | null;
}