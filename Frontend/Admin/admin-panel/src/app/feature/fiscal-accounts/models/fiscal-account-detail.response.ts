import { FolioResponse } from './folio.response';

export interface FiscalAccountDetailResponse {
  id: string;
  originatorId: string;
  cycleIdentifier: string;
  ownerFullName: string;
  createdAt: string;
  folios: FolioResponse[];
}
