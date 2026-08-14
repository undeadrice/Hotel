import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/http/base-http.service';
import { ConfigurationResponse } from '../models/responses/configuration.response';
import { UpsertConfigurationRequest } from '../models/requests/upsert-configuration.request';

@Injectable({
  providedIn: 'root',
})
export class ConfigurationService extends BaseHttpService {
  getConfiguration(): Observable<ConfigurationResponse | null> {
    return this.get<ConfigurationResponse | null>('Configurations');
  }

  upsertConfiguration(request: UpsertConfigurationRequest): Observable<string> {
    return this.post<string>('Configurations', request);
  }

  performEndOfDay(): Observable<string> {
    return this.post<string>('Configurations/end-of-day', {});
  }
}