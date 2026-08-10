import { Component, ChangeDetectionStrategy, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { inject } from '@angular/core';
import { RoomService } from '../../services/room.service';
import { RoomTypeListResponse } from '../../models/responses/room-type-list.response';
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
  });

  readonly roomTypes = signal<RoomTypeListResponse[]>([]);
  readonly loading = signal(true);
  readonly submitting = signal(false);
  readonly isActive = signal(true);

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
