import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { GuestResponse } from '../models/responses/guest.response';
import { GuestListResponse } from '../models/responses/guest-list.response';
import { CreateGuestRequest } from '../models/requests/create-guest.request';
import { UpdateGuestRequest } from '../models/requests/update-guest.request';

@Injectable({
  providedIn: 'root',
})
export class GuestService extends BaseHttpService {
  getGuests(): Observable<GuestListResponse[]> {
    return this.get<GuestListResponse[]>('guests');
  }

  getGuest(id: string): Observable<GuestResponse> {
    return this.get<GuestResponse>(`guests/${id}`);
  }

  createGuest(request: CreateGuestRequest): Observable<string> {
    return this.post<string>('guests', request);
  }

  updateGuest(request: UpdateGuestRequest): Observable<void> {
    return this.put<void>('guests/update', request);
  }
}