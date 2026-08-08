import {
  Component,
  ChangeDetectionStrategy,
  OnInit,
  signal,
  inject,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
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
  templateUrl: './transaction-code-edit.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionCodeEditComponent implements OnInit {
  private fb = inject(FormBuilder);
  private transactionCodeService = inject(TransactionCodeService);
  private transactionGroupService = inject(TransactionGroupService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  readonly transactionGroups = signal<TransactionGroupListResponse[]>([]);
  readonly form: FormGroup = this.fb.group({
    transactionGroupId: ['', [Validators.required]],
    code: ['', [Validators.required, Validators.minLength(2)]],
    name: ['', [Validators.required, Validators.minLength(2)]],
  });

  readonly loading = signal(true);
  readonly submitting = signal(false);

  private transactionCodeId: string | null = null;

  ngOnInit(): void {
    this.transactionCodeId = this.route.snapshot.paramMap.get('id');

    if (!this.transactionCodeId) {
      this.snackBar.open('Invalid transaction code ID', 'Close', {
        duration: 5000,
      });
      this.router.navigate(['/transaction-codes']);
      return;
    }

    this.transactionGroupService
      .getTransactionGroups()
      .subscribe((groups) => {
        this.transactionGroups.set(groups);
      });

    this.transactionCodeService
      .getTransactionCode(this.transactionCodeId)
      .subscribe({
        next: (code) => {
          this.form.patchValue({
            transactionGroupId: code.transactionGroupId,
            code: code.code,
            name: code.name,
          });

          this.loading.set(false);
        },
        error: () => {
          this.snackBar.open('Failed to load transaction code', 'Close', {
            duration: 5000,
          });
          this.router.navigate(['/transaction-codes']);
        },
      });
  }

  onSubmit(): void {
    if (this.form.invalid || !this.transactionCodeId) {
      return;
    }

    this.submitting.set(true);
    this.transactionCodeService
      .updateTransactionCode({
        id: this.transactionCodeId,
        ...this.form.value,
      })
      .subscribe({
        next: () => {
          this.snackBar.open(
            'Transaction code updated successfully',
            'Close',
            {
              duration: 3000,
            }
          );
          this.router.navigate(['/transaction-codes']);
        },
        error: () => {
          this.snackBar.open('Failed to update transaction code', 'Close', {
            duration: 5000,
          });
          this.submitting.set(false);
        },
      });
  }
}
