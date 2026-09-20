import { Component, ChangeDetectionStrategy, signal, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterModule } from '@angular/router';
import { finalize } from 'rxjs';
import { TransactionCodeService } from '../../services/transaction-code.service';
import { TransactionGroupService } from '../../../transaction-groups/services/transaction-group.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { TransactionGroupListResponse } from '../../../transaction-groups/models/responses/transaction-group-list.response';

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
  templateUrl: './transaction-code-add.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionCodeAddComponent {
  private fb = inject(FormBuilder);
  private transactionCodeService = inject(TransactionCodeService);
  private transactionGroupService = inject(TransactionGroupService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  readonly transactionGroups = signal<TransactionGroupListResponse[]>([]);
  readonly form: FormGroup = this.fb.group({
    transactionGroupId: ['', [Validators.required]],
    code: ['', [Validators.required, Validators.minLength(2)]],
    name: ['', [Validators.required, Validators.minLength(2)]],
  });

  readonly submitting = signal(false);

  constructor() {
    this.transactionGroupService
      .getTransactionGroups()
      .subscribe((groups) => {
        this.transactionGroups.set(groups);
      });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);
    this.transactionCodeService
      .createTransactionCode(this.form.value)
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.snackBar.open('Transaction code created successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/transaction-codes']);
      });
  }
}
