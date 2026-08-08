import { Component, ChangeDetectionStrategy, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { inject } from '@angular/core';
import { RoomService } from '../../services/room.service';
import { RoomTypeListResponse } from '../../models/responses/room-type-list.response';
import { RoomStatus } from '../../models/responses/room.response';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';

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
    MatSelectModule,
    MatProgressBarModule,
  ],
  templateUrl: './room-edit.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoomEditComponent implements OnInit {
  private fb = inject(FormBuilder);
  private roomService = inject(RoomService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  readonly form = this.fb.group({
    roomNumber: ['', [Validators.required, Validators.minLength(1)]],
    roomTypeId: ['', [Validators.required]],
    status: [RoomStatus.Available, [Validators.required]],
  });

  readonly roomTypes = signal<RoomTypeListResponse[]>([]);
  readonly loading = signal(true);
  readonly submitting = signal(false);
  readonly isActive = signal(true);

  readonly roomStatuses = Object.values(RoomStatus).filter(
    (v) => typeof v === 'number',
  );
  readonly roomStatusLabels: Record<number, string> = {
    [RoomStatus.Available]: 'Available',
    [RoomStatus.Occupied]: 'Occupied',
    [RoomStatus.Dirty]: 'Dirty',
    [RoomStatus.OutOfService]: 'Out of Service',
    [RoomStatus.Reserved]: 'Reserved',
  };

  private roomId: string | null = null;

  ngOnInit(): void {
    this.roomId = this.route.snapshot.paramMap.get('id');

    if (!this.roomId) {
      this.snackBar.open('Invalid room ID', 'Close', { duration: 5000 });
      this.router.navigate(['/rooms']);
      return;
    }

    forkJoin({
      room: this.roomService.getRoom(this.roomId),
      types: this.roomService.getRoomTypes(),
    }).subscribe({
      next: ({ room, types }) => {
        this.form.patchValue({
          roomNumber: room.roomNumber,
          roomTypeId: room.roomTypeId,
          status: room.status,
        });
        this.isActive.set(room.isActive);
        this.roomTypes.set(types);
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Failed to load room', 'Close', {
          duration: 5000,
        });
        this.router.navigate(['/rooms']);
      },
    });
  }

  onSubmit(): void {
    if (this.form.invalid || !this.roomId) {
      return;
    }

    this.submitting.set(true);
    this.roomService
      .updateRoom({
        id: this.roomId,
        roomNumber: this.form.get('roomNumber')!.value!,
        roomTypeId: this.form.get('roomTypeId')!.value!,
      })
      .subscribe({
        next: () => {
          this.snackBar.open('Room updated successfully', 'Close', {
            duration: 3000,
          });
          this.router.navigate(['/rooms']);
        },
        error: () => {
          this.snackBar.open('Failed to update room', 'Close', {
            duration: 5000,
          });
          this.submitting.set(false);
        },
      });
  }

  onChangeStatus(): void {
    if (!this.roomId) {
      return;
    }

    const newStatus = this.form.get('status')!.value as number;
    this.submitting.set(true);
    this.roomService
      .changeRoomStatus({
        roomId: this.roomId,
        newStatus: newStatus as RoomStatus,
      })
      .subscribe({
        next: () => {
          this.snackBar.open('Room status changed successfully', 'Close', {
            duration: 3000,
          });
          this.submitting.set(false);
        },
        error: () => {
          this.snackBar.open('Failed to change room status', 'Close', {
            duration: 5000,
          });
          this.submitting.set(false);
        },
      });
  }

  onDeactivate(): void {
    if (!this.roomId) {
      return;
    }

    this.submitting.set(true);
    this.roomService.deactivateRoom(this.roomId).subscribe({
      next: () => {
        this.snackBar.open('Room deactivated successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/rooms']);
      },
      error: () => {
        this.snackBar.open('Failed to deactivate room', 'Close', {
          duration: 5000,
        });
        this.submitting.set(false);
      },
    });
  }
}