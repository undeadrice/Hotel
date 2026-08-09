import { FolioItemResponse } from './folio-item.response';

export interface FolioResponse {
  id: string;
  fiscalAccountId: string;
  createdAt: string;
  items: FolioItemResponse[];
}