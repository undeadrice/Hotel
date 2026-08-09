import {
  Component,
  ChangeDetectionStrategy,
  signal,
  inject,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { ReservationService } from '../../services/reservation.service';
import { RoomService } from '../../../rooming/services/room.service';
import { RatePlanService } from '../../../rate-plans/services/rate-plan.service';
import { GuestService } from '../../../guests/services/guest.service';
import { RoomListResponse } from '../../../rooming/models/responses/room-list.response';
import { RatePlanListResponse } from '../../../rate-plans/models/responses/rate-plan-list.response';
import { GuestListResponse } from '../../../guests/models/responses/guest-list.response';
import { CreateReservationRequest } from '../../models/requests/create-reservation.request';

@Component({
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  templateUrl: './reservation-add.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReservationAddComponent {
  private readonly fb = inject(FormBuilder);
  private readonly reservationService = inject(ReservationService);
  private readonly roomService = inject(RoomService);
  private readonly ratePlanService = inject(RatePlanService);
  private readonly guestService = inject(GuestService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly router = inject(Router);

  readonly rooms = signal<RoomListResponse[]>([]);
  readonly ratePlans = signal<RatePlanListResponse[]>([]);
  readonly guests = signal<GuestListResponse[]>([]);

  readonly form: FormGroup = this.fb.group({
    creatorId: ['', [Validators.required]],
    roomId: ['', [Validators.required]],
    ratePlanId: ['', [Validators.required]],
    startDate: ['', [Validators.required]],
    endDate: ['', [Validators.required]],
    arrivalTime: [''],
    guestIds: [[], [Validators.required]],
  });

  readonly submitting = signal(false);

  constructor() {
    this.roomService.getRooms().subscribe((data) => {
      this.rooms.set(data);
    });

    this.ratePlanService.getRatePlans().subscribe((data) => {
      this.ratePlans.set(data);
    });

    this.guestService.getGuests().subscribe((data) => {
      this.guests.set(data);
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);

    const formValue = this.form.value;
    const request: CreateReservationRequest = {
      creatorId: formValue.creatorId,
      roomId: formValue.roomId,
      ratePlanId: formValue.ratePlanId,
      startDate: formValue.startDate,
      endDate: formValue.endDate,
      arrivalTime: formValue.arrivalTime || null,
      guestIds: formValue.guestIds,
    };

    this.reservationService.createReservation(request).subscribe({
      next: () => {
        this.snackBar.open('Reservation created successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/reservations']);
      },
      error: () => {
        this.snackBar.open('Failed to create reservation', 'Close', {
          duration: 5000,
        });
        this.submitting.set(false);
      },
    });
  }
}