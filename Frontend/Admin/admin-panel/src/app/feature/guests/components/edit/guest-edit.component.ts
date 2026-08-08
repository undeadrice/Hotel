import { Component, ChangeDetectionStrategy, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { inject } from '@angular/core';
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
  templateUrl: './guest-edit.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GuestEditComponent implements OnInit {
  private fb = inject(FormBuilder);
  private guestService = inject(GuestService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  readonly form: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.minLength(2)]],
    lastName: ['', [Validators.required, Validators.minLength(2)]],
    phone: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    documentNumber: ['', [Validators.required]],
  });

  readonly loading = signal(true);
  readonly submitting = signal(false);

  private guestId: string | null = null;

  ngOnInit(): void {
    this.guestId = this.route.snapshot.paramMap.get('id');

    if (!this.guestId) {
      this.snackBar.open('Invalid guest ID', 'Close', { duration: 5000 });
      this.router.navigate(['/guests']);
      return;
    }

    this.guestService.getGuest(this.guestId).subscribe({
      next: (guest) => {
        this.form.patchValue({
          firstName: guest.firstName,
          lastName: guest.lastName,
          phone: guest.phone,
          email: guest.email,
          documentNumber: guest.documentNumber,
        });

        this.loading.set(false);
      },
      error: () => {
        this.snackBar.open('Failed to load guest', 'Close', {
          duration: 5000,
        });
        this.router.navigate(['/guests']);
      },
    });
  }

  onSubmit(): void {
    if (this.form.invalid || !this.guestId) {
      return;
    }

    this.submitting.set(true);
    this.guestService
      .updateGuest({
        id: this.guestId,
        ...this.form.value,
      })
      .subscribe({
        next: () => {
          this.snackBar.open('Guest updated successfully', 'Close', {
            duration: 3000,
          });
          this.router.navigate(['/guests']);
        },
        error: () => {
          this.snackBar.open('Failed to update guest', 'Close', {
            duration: 5000,
          });
          this.submitting.set(false);
        },
      });
  }
}