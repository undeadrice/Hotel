import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { SHARED_IMPORTS } from '../../../../../shared-module';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ConfigurationService } from '../../services/configuration.service';

interface TimeZoneOption {
  value: string;
  label: string;
}

@Component({
  imports: [
    ...SHARED_IMPORTS,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  templateUrl: './setup.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SetupComponent implements OnInit {
  private readonly configurationService = inject(ConfigurationService);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  readonly submitting = signal(false);
  readonly errorMessage = signal('');

  readonly timeZones = signal<TimeZoneOption[]>([]);

  readonly form = this.fb.group({
    timeZoneId: ['', [Validators.required]],
    currentBusinessDate: [new Date(), [Validators.required]],
    seedBusinessData: [true],
  });

  ngOnInit(): void {
    this.loadTimeZones();
  }

  submit(): void {
    if (this.form.invalid) {
      return;
    }

    this.submitting.set(true);
    this.errorMessage.set('');

    const value = this.form.value;

    this.configurationService
      .seedData({
        timeZoneId: value.timeZoneId ?? '',
        currentBusinessDate: this.toDateOnly(value.currentBusinessDate),
        seedBusinessData: value.seedBusinessData ?? false,
      })
      .pipe(finalize(() => this.submitting.set(false)))
      .subscribe({
        next: () => {
          this.router.navigate(['/roles']);
        },
        error: () => {
          this.errorMessage.set('Something went wrong while setting up the system.');
        },
      });
  }

  private loadTimeZones(): void {
    this.configurationService.getServerTimeZones().subscribe((timeZones) => {
      this.timeZones.set(
        timeZones.map((tz) => ({
          value: tz.id,
          label: tz.displayName,
        })),
      );
    });
  }

  private toDateOnly(value: unknown): string {
    if (value instanceof Date) {
      const year = value.getFullYear();
      const month = String(value.getMonth() + 1).padStart(2, '0');
      const day = String(value.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    }

    if (typeof value === 'string') {
      return value;
    }

    return '';
  }
}