import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReservationService } from '../../services/reservation.service';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { ReservationListResponse } from '../../models/responses/reservation-list.response';
import { ReservationStatus } from '../../enums/reservation-status.enum';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './reservation-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReservationListComponent {
  private readonly reservationService = inject(ReservationService);
  private readonly snackBar = inject(MatSnackBar);

  reservations = new MatTableDataSource<ReservationListResponse>([]);
  displayedColumns: string[] = [
    'CycleIdentifier',
    'RoomName',
    'RatePlanName',
    'CreatorName',
    'StartDate',
    'EndDate',
    'ArrivalTime',
    'Status',
    'CreatedAt',
    'GuestCount',
    'Actions',
  ];

  constructor() {
    this.loadReservations();
  }

  statusLabel(status: ReservationStatus): string {
    switch (status) {
      case ReservationStatus.Reserved:
        return 'Reserved';
      case ReservationStatus.DueIn:
        return 'Due In';
      case ReservationStatus.InHouse:
        return 'In House';
      case ReservationStatus.CheckedOut:
        return 'Checked Out';
      case ReservationStatus.NoShow:
        return 'No Show';
      default:
        return 'Invalid';
    }
  }

  canCheckIn(status: ReservationStatus): boolean {
    return status === ReservationStatus.DueIn;
  }

  checkIn(reservation: ReservationListResponse): void {
    this.reservationService.checkIn(reservation.id).subscribe({
      next: () => {
        this.snackBar.open('Reservation checked in', 'Close', { duration: 3000 });
        this.loadReservations();
      },
    });
  }

  private loadReservations(): void {
    this.reservationService.getReservations().subscribe((data) => {
      this.reservations.data = data;
    });
  }
}