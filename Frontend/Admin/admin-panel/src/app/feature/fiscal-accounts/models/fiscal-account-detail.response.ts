import { FiscalAccountStatus } from '../enums/fiscal-account-status.enum';
import { FolioResponse } from './folio.response';

export interface FiscalAccountDetailResponse {
  id: string;
  originatorId: string;
  cycleIdentifier: string;
  ownerFullName: string;
  createdAt: string;
  status: FiscalAccountStatus;
  folios: FolioResponse[];
}
