export interface RoomResponse {
  id: string;
  roomNumber: string;
  roomTypeId: string;
  roomTypeName: string;
  status: RoomStatus;
  isActive: boolean;
}

export enum RoomStatus {
  Available = 0,
  Occupied = 1,
  Dirty = 2,
  OutOfService = 3,
  Reserved = 4,
}