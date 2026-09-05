import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { finalize } from 'rxjs';
import { ConfigurationService } from '../services/configuration.service';
import { SHARED_IMPORTS } from '../../../../shared-module';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

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
  templateUrl: './configuration.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfigurationComponent implements OnInit {
  private readonly configurationService = inject(ConfigurationService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly fb = inject(FormBuilder);

  readonly loading = signal(true);
  readonly saving = signal(false);

  readonly timeZones = signal<TimeZoneOption[]>([]);

  readonly form: FormGroup = this.fb.group({
    timeZoneId: ['', [Validators.required]],
    currentBusinessDate: [this.today(), [Validators.required]],
  });

  ngOnInit(): void {
    this.loadTimeZones();
    this.loadConfiguration();
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    this.saving.set(true);

    const value = this.form.value;
    const currentBusinessDate = this.toDateOnly(value.currentBusinessDate);

    this.configurationService
      .upsertConfiguration({
        timeZoneId: value.timeZoneId,
        currentBusinessDate,
      })
      .pipe(finalize(() => this.saving.set(false)))
      .subscribe(() => {
        this.snackBar.open('Configuration saved successfully', 'Close', { duration: 3000 });
      });
  }

  private loadTimeZones(): void {
    this.configurationService.getServerTimeZones().subscribe((timeZones) => {
      this.timeZones.set(
        timeZones.map((tz) => ({
          value: tz.id,
          label: tz.displayName,
        }))
      );
    });
  }

  private loadConfiguration(): void {
    this.configurationService
      .getConfiguration()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((configuration) => {
        if (configuration) {
          this.form.patchValue({
            timeZoneId: configuration.timeZoneId,
            currentBusinessDate: configuration.currentBusinessDate,
          });
        }
      });
  }

  private today(): Date {
    return new Date();
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