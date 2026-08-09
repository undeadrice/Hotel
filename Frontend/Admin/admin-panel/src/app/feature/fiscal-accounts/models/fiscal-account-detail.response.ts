import { FolioResponse } from './folio.response';

export interface FiscalAccountDetailResponse {
  id: string;
  originatorId: string;
  ownerId: string;
  createdAt: string;
  folios: FolioResponse[];
}