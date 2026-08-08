import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
import { inject } from '@angular/core';
import { RoomService } from '../../services/room.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
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
    MatProgressBarModule,
  ],
  templateUrl: './room-type-add.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoomTypeAddComponent {
  private fb = inject(FormBuilder);
  private roomService = inject(RoomService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  readonly form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    baseRate: [0, [Validators.required, Validators.min(0)]],
    description: [''],
  });

  readonly submitting = signal(false);

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);
    this.roomService
      .createRoomType({
        name: this.form.get('name')!.value!,
        baseRate: this.form.get('baseRate')!.value!,
        description: this.form.get('description')!.value || null,
      })
      .subscribe({
        next: () => {
          this.snackBar.open('Room type created successfully', 'Close', {
            duration: 3000,
          });
          this.router.navigate(['/room-types']);
        },
        error: () => {
          this.submitting.set(false);
        },
      });
  }
}