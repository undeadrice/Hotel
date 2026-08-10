import {
  Component,
  ChangeDetectionStrategy,
  signal,
  inject,
  DestroyRef,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
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
import { combineLatest, startWith } from 'rxjs';
import { ReservationService } from '../../services/reservation.service';
import { RoomService } from '../../../rooming/services/room.service';
import { RatePlanService } from '../../../rate-plans/services/rate-plan.service';
import { GuestService } from '../../../guests/services/guest.service';
import { RoomListResponse } from '../../../rooming/models/responses/room-list.response';
import { RatePlanListSimpleResponse } from '../../../rate-plans/models/responses/rate-plan-list-simple.response';
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
  private readonly destroyRef = inject(DestroyRef);

  readonly rooms = signal<RoomListResponse[]>([]);
  readonly ratePlans = signal<RatePlanListSimpleResponse[]>([]);
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

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  constructor() {
    this.guestService.getGuests().subscribe((data) => {
      this.guests.set(data);
    });

    const startDate$ = this.form.get('startDate')!.valueChanges.pipe(startWith(null));
    const endDate$ = this.form.get('endDate')!.valueChanges.pipe(startWith(null));
    const roomId$ = this.form.get('roomId')!.valueChanges.pipe(startWith(null));

    // When both dates are set, fetch available rooms
    combineLatest([startDate$, endDate$])
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(([startDate, endDate]) => {
        if (startDate && endDate) {
          const start = this.formatDate(startDate);
          const end = this.formatDate(endDate);

          this.roomService.getAvailableRooms(start, end).subscribe((data) => {
            this.rooms.set(data);
            const currentRoomId = this.form.get('roomId')?.value;
            if (currentRoomId && !data.some((r) => r.id === currentRoomId)) {
              this.form.patchValue({ roomId: '', ratePlanId: '' }, { emitEvent: false });
              this.ratePlans.set([]);
            }
          });
        } else {
          this.rooms.set([]);
          this.ratePlans.set([]);
          this.form.patchValue({ roomId: '', ratePlanId: '' }, { emitEvent: false });
        }
      });

    // When a room is selected, fetch rate plans configured for its room type
    roomId$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((roomId) => {
        if (roomId) {
          this.ratePlanService.getRatePlansByRoom(roomId).subscribe((data) => {
            this.ratePlans.set(data);
            const currentRatePlanId = this.form.get('ratePlanId')?.value;
            if (currentRatePlanId && !data.some((rp) => rp.id === currentRatePlanId)) {
              this.form.patchValue({ ratePlanId: '' }, { emitEvent: false });
            }
          });
        } else {
          this.ratePlans.set([]);
          this.form.patchValue({ ratePlanId: '' }, { emitEvent: false });
        }
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