import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { RoomTypeListResponse } from '../models/responses/room-type-list.response';
import { RoomTypeResponse } from '../models/responses/room-type.response';
import { CreateRoomTypeRequest } from '../models/requests/create-room-type.request';
import { UpdateRoomTypeRequest } from '../models/requests/update-room-type.request';

@Injectable({
  providedIn: 'root',
})
export class RoomTypeService extends BaseHttpService {
  getRoomTypes(): Observable<RoomTypeListResponse[]> {
    return this.get<RoomTypeListResponse[]>('RoomTypes');
  }

  getRoomType(id: string): Observable<RoomTypeResponse> {
    return this.get<RoomTypeResponse>(`RoomTypes/${id}`);
  }

  createRoomType(request: CreateRoomTypeRequest): Observable<string> {
    return this.post<string>('RoomTypes', request);
  }

  updateRoomType(request: UpdateRoomTypeRequest): Observable<void> {
    return this.put<void>('RoomTypes', request);
  }
}