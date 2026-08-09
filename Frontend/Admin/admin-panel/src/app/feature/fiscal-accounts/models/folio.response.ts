import { FolioItemResponse } from './folio-item.response';

export interface FolioResponse {
  id: string;
  createdAt: string;
  items: FolioItemResponse[];
}
