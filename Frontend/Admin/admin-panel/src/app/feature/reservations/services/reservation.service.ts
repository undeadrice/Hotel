import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { ReservationResponse } from '../models/responses/reservation.response';
import { ReservationListResponse } from '../models/responses/reservation-list.response';
import { CreateReservationRequest } from '../models/requests/create-reservation.request';

@Injectable({
  providedIn: 'root',
})
export class ReservationService extends BaseHttpService {
  getReservations(): Observable<ReservationListResponse[]> {
    return this.get<ReservationListResponse[]>('Reservations');
  }

  getReservation(id: string): Observable<ReservationResponse> {
    return this.get<ReservationResponse>(`Reservations/${id}`);
  }

  createReservation(request: CreateReservationRequest): Observable<string> {
    return this.post<string>('Reservations', request);
  }

  checkIn(id: string): Observable<void> {
    return this.post<void>(`Reservations/${id}/check-in`, {});
  }
}
