import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { RoomTypeService } from '../../services/room-type.service';
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
  private roomTypeService = inject(RoomTypeService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  readonly form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    description: [''],
  });

  readonly submitting = signal(false);

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);
    this.roomTypeService
      .createRoomType({
        name: this.form.get('name')!.value!,
        description: this.form.get('description')!.value || null,
      })
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.snackBar.open('Room type created successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/room-types']);
      });
  }
}