import { FiscalAccountStatus } from '../enums/fiscal-account-status.enum';

export interface FiscalAccountListResponse {
  id: string;
  cycleIdentifier: string;
  ownerFullName: string;
  createdAt: string;
  status: FiscalAccountStatus;
}
