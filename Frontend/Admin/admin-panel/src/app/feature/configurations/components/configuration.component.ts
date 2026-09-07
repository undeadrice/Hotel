import { Component, ChangeDetectionStrategy, inject, signal, computed, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { ConfigurationService } from '../services/configuration.service';
import { ConfigurationResponse } from '../models/responses/configuration.response';
import { TimeZoneResponse } from '../models/responses/time-zone.response';
import { SHARED_IMPORTS } from '../../../../shared-module';

@Component({
  imports: [...SHARED_IMPORTS],
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfigurationComponent implements OnInit {
  private readonly configurationService = inject(ConfigurationService);

  readonly loading = signal(true);
  readonly configuration = signal<ConfigurationResponse | null>(null);
  readonly timeZones = signal<TimeZoneResponse[]>([]);

  readonly timeZoneDisplayName = computed(() => {
    const configuration = this.configuration();
    if (!configuration) {
      return '';
    }

    return (
      this.timeZones().find((timeZone) => timeZone.id === configuration.timeZoneId)?.displayName ??
      configuration.timeZoneId
    );
  });

  readonly businessDateDisplay = computed(() => {
    const value = this.configuration()?.currentBusinessDate;
    if (!value) {
      return '';
    }

    return new Date(`${value}T00:00:00`).toLocaleDateString(undefined, {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  });

  ngOnInit(): void {
    this.loadConfiguration();
    this.loadTimeZones();
  }

  private loadConfiguration(): void {
    this.configurationService
      .getConfiguration()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((configuration) => {
        if (configuration) {
          this.configuration.set(configuration);
        }
      });
  }

  private loadTimeZones(): void {
    this.configurationService.getServerTimeZones().subscribe((timeZones) => {
      this.timeZones.set(timeZones);
    });
  }
}