import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { RoomListResponse } from '../models/responses/room-list.response';
import { RoomResponse } from '../models/responses/room.response';
import { RoomTypeListResponse } from '../models/responses/room-type-list.response';
import { RoomTypeResponse } from '../models/responses/room-type.response';
import { CreateRoomRequest } from '../models/requests/create-room.request';
import { UpdateRoomRequest } from '../models/requests/update-room.request';
import { CreateRoomTypeRequest } from '../models/requests/create-room-type.request';
import { UpdateRoomTypeRequest } from '../models/requests/update-room-type.request';
@Injectable({
  providedIn: 'root',
})
export class RoomService extends BaseHttpService {
  getRooms(): Observable<RoomListResponse[]> {
    return this.get<RoomListResponse[]>('rooms');
  }

  getAvailableRooms(startDate: string, endDate: string): Observable<RoomListResponse[]> {
    return this.get<RoomListResponse[]>(
      `rooms/available?startDate=${startDate}&endDate=${endDate}`
    );
  }

  getRoom(id: string): Observable<RoomResponse> {
    return this.get<RoomResponse>(`rooms/${id}`);
  }

  createRoom(request: CreateRoomRequest): Observable<string> {
    return this.post<string>('rooms', request);
  }

  updateRoom(request: UpdateRoomRequest): Observable<void> {
    return this.put<void>('rooms', request);
  }

  deactivateRoom(roomId: string): Observable<void> {
    return this.put<void>('rooms/deactivate', { roomId });
  }

  getRoomTypes(): Observable<RoomTypeListResponse[]> {
    return this.get<RoomTypeListResponse[]>('rooms/types');
  }

  getRoomType(id: string): Observable<RoomTypeResponse> {
    return this.get<RoomTypeResponse>(`rooms/types/${id}`);
  }

  createRoomType(request: CreateRoomTypeRequest): Observable<string> {
    return this.post<string>('rooms/types', request);
  }

  updateRoomType(request: UpdateRoomTypeRequest): Observable<void> {
    return this.put<void>('rooms/types', request);
  }
}