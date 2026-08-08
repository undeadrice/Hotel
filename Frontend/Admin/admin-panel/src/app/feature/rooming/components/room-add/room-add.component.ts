import { Component, ChangeDetectionStrategy, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
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
  templateUrl: './room-add.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoomAddComponent implements OnInit {
  private fb = inject(FormBuilder);
  private roomService = inject(RoomService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  readonly form = this.fb.group({
    roomNumber: ['', [Validators.required, Validators.minLength(1)]],
    roomTypeId: ['', [Validators.required]],
  });

  readonly roomTypes = signal<RoomTypeListResponse[]>([]);
  readonly loading = signal(true);
  readonly submitting = signal(false);

  ngOnInit(): void {
    this.roomService.getRoomTypes().subscribe({
      next: (types) => {
        this.roomTypes.set(types);
        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Failed to load room types', 'Close', {
          duration: 5000,
        });
        this.loading.set(false);
      },
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);
    this.roomService
      .createRoom({
        roomNumber: this.form.get('roomNumber')!.value!,
        roomTypeId: this.form.get('roomTypeId')!.value!,
      })
      .subscribe({
        next: () => {
          this.snackBar.open('Room created successfully', 'Close', {
            duration: 3000,
          });
          this.router.navigate(['/rooms']);
        },
        error: () => {
          this.submitting.set(false);
        },
      });
  }
}