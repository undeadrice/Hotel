import { RoomStatus } from '../responses/room.response';

export interface ChangeRoomStatusRequest {
  roomId: string;
  newStatus: RoomStatus;
}