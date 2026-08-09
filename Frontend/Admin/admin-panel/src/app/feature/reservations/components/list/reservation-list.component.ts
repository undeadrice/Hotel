import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ReservationService } from '../../services/reservation.service';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { ReservationListResponse } from '../../models/responses/reservation-list.response';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './reservation-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReservationListComponent {
  private readonly reservationService = inject(ReservationService);
  private readonly router = inject(Router);

  reservations = new MatTableDataSource<ReservationListResponse>([]);
  displayedColumns: string[] = [
    'RoomId',
    'RatePlanId',
    'StartDate',
    'EndDate',
    'ArrivalTime',
    'GuestCount',
  ];

  constructor() {
    this.reservationService.getReservations().subscribe((data) => {
      this.reservations.data = data;
    });
  }
}