import {
  Component,
  ChangeDetectionStrategy,
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
import { Router, RouterModule } from '@angular/router';
import { finalize } from 'rxjs';
import { NumberCycleService } from '../../services/number-cycle.service';
import { NumberCycleTopic } from '../../enums/number-cycle-topic.enum';
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
  templateUrl: './number-cycle-add.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NumberCycleAddComponent {
  private fb = inject(FormBuilder);
  private numberCycleService = inject(NumberCycleService);
  private snackBar = inject(MatSnackBar);
  private router = inject(Router);

  readonly form: FormGroup = this.fb.group({
    topic: [NumberCycleTopic.Reservation, [Validators.required]],
    prefix: ['', [Validators.required, Validators.minLength(1)]],
    startIndex: [1, [Validators.required, Validators.min(1)]],
  });

  readonly submitting = signal(false);
  readonly topicValues = computed(() =>
    Object.entries(NumberCycleTopic)
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
    this.numberCycleService
      .createNumberCycle(this.form.value)
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe(() => {
        this.snackBar.open('Number cycle created successfully', 'Close', {
          duration: 3000,
        });
        this.router.navigate(['/number-cycles']);
      });
  }
}