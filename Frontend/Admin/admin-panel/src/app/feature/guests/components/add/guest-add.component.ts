import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { GuestService } from '../../services/guest.service';
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
  templateUrl: './guest-add.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GuestAddComponent {
  private fb = inject(FormBuilder);
  private guestService = inject(GuestService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  readonly form: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(2)]],
    lastName: ['', [Validators.required, Validators.minLength(2)]],
    phone: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    documentNumber: ['', [Validators.required]],
  });

  readonly submitting = signal(false);

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);
    this.guestService
      .createGuest(this.form.value)
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.snackBar.open('Guest created successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/guests']);
      });
  }
}