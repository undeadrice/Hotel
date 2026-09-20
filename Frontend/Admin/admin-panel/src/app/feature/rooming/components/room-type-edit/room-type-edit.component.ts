import { Component, ChangeDetectionStrategy, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
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
  templateUrl: './room-type-edit.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoomTypeEditComponent implements OnInit {
  private fb = inject(FormBuilder);
  private roomTypeService = inject(RoomTypeService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  readonly form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    description: [''],
  });

  readonly loading = signal(true);
  readonly submitting = signal(false);

  private roomTypeId: string | null = null;

  ngOnInit(): void {
    this.roomTypeId = this.route.snapshot.paramMap.get('id');

    if (!this.roomTypeId) {
      this.snackBar.open('Invalid room type ID', 'Close', { duration: 5000 });
      this.router.navigate(['/room-types']);
      return;
    }

    this.roomTypeService
      .getRoomType(this.roomTypeId)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((roomType) => {
        this.form.patchValue({
          name: roomType.name,
          description: roomType.description ?? '',
        });
      });
  }

  onSubmit(): void {
    if (this.form.invalid || !this.roomTypeId) {
      return;
    }

    this.submitting.set(true);
    this.roomTypeService
      .updateRoomType({
        id: this.roomTypeId,
        name: this.form.get('name')!.value!,
        description: this.form.get('description')!.value || null,
      })
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.snackBar.open('Room type updated successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/room-types']);
      });
  }
}