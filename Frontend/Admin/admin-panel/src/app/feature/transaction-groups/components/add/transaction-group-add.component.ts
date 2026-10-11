import { Component, ChangeDetectionStrategy, signal, inject, computed } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
import { finalize } from 'rxjs';
import { TransactionGroupService } from '../../services/transaction-group.service';
import { TransactionType } from '../../enums/transaction-type.enum';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
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
    MatSelectModule,
  ],
  templateUrl: './transaction-group-add.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionGroupAddComponent {
  private fb = inject(FormBuilder);
  private transactionGroupService = inject(TransactionGroupService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  readonly form: FormGroup = this.fb.group({
    code: ['', [Validators.required, Validators.minLength(2)]],
    name: ['', [Validators.required, Validators.minLength(2)]],
    type: [TransactionType.Charge, [Validators.required]],
  });

  readonly submitting = signal(false);
  readonly transactionTypeValues = computed(() =>
    Object.entries(TransactionType)
      .filter(([key]) => isNaN(Number(key)))
      .map(([key, value]) => ({
        label: key,
        value: value as number,
      }))
  );

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);
    this.transactionGroupService
      .createTransactionGroup(this.form.value)
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.snackBar.open('Transaction group created successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/transaction-groups']);
      });
  }
}
