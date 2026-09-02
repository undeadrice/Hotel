import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { ConfigurationResponse } from '../models/responses/configuration.response';
import { TimeZoneResponse } from '../models/responses/time-zone.response';
import { UpsertConfigurationRequest } from '../models/requests/upsert-configuration.request';
import { SeedDataRequest } from '../models/requests/seed-data.request';

@Injectable({
  providedIn: 'root',
})
export class ConfigurationService extends BaseHttpService {
  getConfiguration(): Observable<ConfigurationResponse | null> {
    return this.get<ConfigurationResponse | null>('Configurations');
  }

  getServerTimeZones(): Observable<TimeZoneResponse[]> {
    return this.get<TimeZoneResponse[]>('Configurations/time-zones');
  }

  upsertConfiguration(request: UpsertConfigurationRequest): Observable<string> {
    return this.post<string>('Configurations', request);
  }

  seedData(request: SeedDataRequest): Observable<string> {
    return this.post<string>('Configurations/seed', request);
  }

  getSeedStatus(): Observable<boolean> {
    return this.get<boolean>('Configurations/seed-status');
  }

  performEndOfDay(): Observable<string> {
    return this.post<string>('Configurations/end-of-day', {});
  }
}