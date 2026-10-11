import {
  Component,
  ChangeDetectionStrategy,
  OnInit,
  signal,
  inject,
  computed,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
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
import { MatTableModule } from '@angular/material/table';
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
    MatTableModule,
  ],
  templateUrl: './transaction-group-edit.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransactionGroupEditComponent implements OnInit {
  private fb = inject(FormBuilder);
  private transactionGroupService = inject(TransactionGroupService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  readonly form: FormGroup = this.fb.group({
    code: ['', [Validators.required, Validators.minLength(2)]],
    name: ['', [Validators.required, Validators.minLength(2)]],
    type: [TransactionType.Charge, [Validators.required]],
  });

  readonly loading = signal(true);
  readonly submitting = signal(false);
  readonly transactionCodes = signal<
    Array<{
      id: string;
      code: string;
      name: string;
      defaultAmount: number;
      isActive: boolean;
    }>
  >([]);
  readonly transactionTypeValues = computed(() =>
    Object.entries(TransactionType)
      .filter(([key]) => isNaN(Number(key)))
      .map(([key, value]) => ({
        label: key,
        value: value as number,
      }))
  );

  private transactionGroupId: string | null = null;

  ngOnInit(): void {
    this.transactionGroupId = this.route.snapshot.paramMap.get('id');

    if (!this.transactionGroupId) {
      this.snackBar.open('Invalid transaction group ID', 'Close', {
        duration: 5000,
      });
      this.router.navigate(['/transaction-groups']);
      return;
    }

    this.transactionGroupService
      .getTransactionGroup(this.transactionGroupId)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((group) => {
        this.form.patchValue({
          code: group.code,
          name: group.name,
          type: group.type,
        });

        this.transactionCodes.set(group.transactionCodes ?? []);
      });
  }

  onSubmit(): void {
    if (this.form.invalid || !this.transactionGroupId) {
      return;
    }

    this.submitting.set(true);
    this.transactionGroupService
      .updateTransactionGroup({
        id: this.transactionGroupId,
        ...this.form.value,
      })
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.snackBar.open(
          'Transaction group updated successfully',
          'Close',
          {
            duration: 3000,
          }
        );
        this.router.navigate(['/transaction-groups']);
      });
  }
}
